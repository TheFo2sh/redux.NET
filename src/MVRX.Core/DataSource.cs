using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;
using MVRX.Core.Commands;

namespace MVRX.Core
{
    public class AsyncDataSource<TState, TReducer> : DataSource<TState, TReducer> where TReducer : Reducer<ObservableCollection<TState>>
    {
        public AsyncDataSource(Predicate<DataSource<TState, TReducer>> availabilityPredicate,
       Task<IEnumerable<TState>> dataFunc):base(availabilityPredicate,(() => new List<TState>()))
        {
            dataFunc.ContinueWith(task =>
            {
                foreach (var state in task.Result)
                {
                    this.Add(state);
                }
            });
        }
    }
    public class DataSource<TState, TReducer> : ObservableCollection<TState>,
        IDisposable, IDictionary<string, object>, IStore<ObservableCollection<TState>, TReducer> where TReducer : Reducer<ObservableCollection<TState>>

    {
        private readonly object _syncRoot = new object();
        private readonly IList<IActionCommand> _actionCommands;

        private readonly IDictionary<string, object> _dictionaryImplementation;
        public IEnumerable<TState> Data { get; set; }
        private readonly ReplaySubject<ObservableCollection<TState>> _stateSubject = new ReplaySubject<ObservableCollection<TState>>(1);

        public virtual void Insert(TState item)
        {
        }

        public virtual void Delete(TState item)
        {
        }

        private readonly Dispatcher _dispatcher;
        private readonly TReducer _reducer;
        public Predicate<DataSource<TState, TReducer>> AvailabilityPredicate { get; set; }

   
        public DataSource(Predicate<DataSource<TState, TReducer>> availabilityPredicate, Func<IEnumerable<TState>> dataFunc) :
            base(dataFunc.Invoke())
        {
            this.AvailabilityPredicate = availabilityPredicate;
            _dictionaryImplementation = new Dictionary<string, object>();
            dependentProperties = new List<string>();
            this.CollectionChanged += OnCollectionChanged;
            activeQuerys = new List<QueryObject<TState,TReducer>>();
            Data = this.ToList<TState>();
            _dispatcher = InnerDispatch;
            _reducer = ServiceLocator.Current.GetInstance<TReducer>();
            _actionCommands=new List<IActionCommand>();
        }

        private void OnCollectionChanged(object sender,
            NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            RefreshProperties();

            if (IsSilent)
                return;

            var newItems = notifyCollectionChangedEventArgs.NewItems?.Cast<TState>()?.ToList() ?? new List<TState>();
            var oldItems = notifyCollectionChangedEventArgs.OldItems?.Cast<TState>()?.ToList() ?? new List<TState>();

            switch (notifyCollectionChangedEventArgs.Action)
            {
                case NotifyCollectionChangedAction.Add:
                {
                    Insert(newItems.FirstOrDefault(item => !oldItems.Contains(item)));
                    break;
                }
                case NotifyCollectionChangedAction.Remove:
                {
                    Remove(oldItems.FirstOrDefault(item => !newItems.Contains(item)));
                    break;
                }
            }
            Data = newItems;
        }


        public void Dispose()
        {
            this.CollectionChanged -= OnCollectionChanged;

            this.Clear();
            GC.Collect();
        }


        private readonly List<QueryObject<TState,TReducer>> activeQuerys;
        public List<QueryObject<TState,TReducer>> Queries { get; set; }

        public void AddQuery(QueryObject<TState,TReducer> queryObject)
        {
            _dictionaryImplementation.Add(queryObject.GetType().Name, queryObject);
        }

        public void ExcuteFilter(QueryObject<TState,TReducer> queryObject)
        {
            if (activeQuerys.LastOrDefault()?.GroupName != queryObject.GroupName)
                activeQuerys.Clear();

            var oldQuery = activeQuerys.FirstOrDefault(x => x.GetType() == queryObject.GetType());
            if (oldQuery != null)
                activeQuerys.Remove(oldQuery);

            activeQuerys.Add(queryObject);

            FireQuery();
        }

        private void FireQuery()
        {
            var dataSource = Data.ToList();

            foreach (var queryObject in activeQuerys)
            {
                this.IsSilent = true;


                dataSource = queryObject.Query(dataSource).ToList();
            }
            this.Clear();
            foreach (var x1 in dataSource)
            {
                this.Add(x1);
            }
            this.IsSilent = false;
        }

        private void RefreshProperties()
        {
            foreach (var dependentProperty in dependentProperties)
            {
                object result;
                var bindablecollection = (ObservableCollection<object>) _dictionaryImplementation[dependentProperty];
                bindablecollection.Clear();
                TryGetPropertyValues(dependentProperty, out result);
                var resultList = (ObservableCollection<object>) result;
                foreach (var o in resultList)
                {
                    bindablecollection.Add(o);
                }
            }

        }

        private List<string> dependentProperties;
        private bool IsSilent { get; set; }

        public new IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _dictionaryImplementation.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _dictionaryImplementation).GetEnumerator();
        }

        void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item)
        {
            _dictionaryImplementation.Add(item);
        }

        bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> item)
        {
            return _dictionaryImplementation.Contains(item);
        }

        void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            _dictionaryImplementation.CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item)
        {
            return _dictionaryImplementation.Remove(item);
        }

        public new int Count
        {
            get { return _dictionaryImplementation.Count; }
        }

        public  bool IsReadOnly
        {
            get { return _dictionaryImplementation.IsReadOnly; }
        }

        void IDictionary<string, object>.Add(string key, object value)
        {
            _dictionaryImplementation.Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return true;
        }

        bool IDictionary<string, object>.Remove(string key)
        {
            return _dictionaryImplementation.Remove(key);
        }

        bool IDictionary<string, object>.TryGetValue(string key, out object value)
        {
            var result = _dictionaryImplementation.TryGetValue(key, out value);
            if (!result)
            {
                result = this.TryGetPropertyValues(key, out value);
            }
            return result;
        }

       

        private bool TryGetPropertyValues(string originalKey, out object value)
        {
            value = null;
            var dataSource = Data;
            var key = originalKey;
            if (key.ToLower().StartsWith("selected"))
            {
                dataSource = this.ToList<TState>();
                key = key.Remove(0, 8);
            }
            var property = typeof(TState).GetProperty(key);
            try
            {
                if (property == null)
                {
                    return false;
                }
                value = new ObservableCollection<object>((dataSource.Select(x => property.GetValue(x)).Distinct()));

                if(!_dictionaryImplementation.ContainsKey(originalKey))
                _dictionaryImplementation.Add(originalKey,value);

                if (!dependentProperties.Contains(originalKey))
                    dependentProperties.Add(originalKey);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

       
        public object this[string key]
        {
            get
            {
                object result;
                TryGetPropertyValues(key, out result);
                return result;
            }
            set
            {
                
            }
        }

        public ICollection<string> Keys
        {
            get { return _dictionaryImplementation.Keys; }
        }

        public ICollection<object> Values
        {
            get { return _dictionaryImplementation.Values; }
        }

        public void VariableSubscribe(Action<object> obj)
        {
            this.Subscribe(s => obj.Invoke(s));
        }

        public IAction Dispatch(IAction action)
        {
            return _dispatcher.Invoke(action);
        }

        public bool IsValid(IAction action)
        {
            try
            {
                
                var newState = _reducer.Reduce(new ObservableCollection<TState>(Data), action);
                return _reducer.Validate(new ObservableCollection<TState>(Data), newState, action);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private ObservableCollection<TState> CreateObservableDataObject()
        {
            var ObservableDataObject = new ObservableCollection<TState>(Data);
            ObservableDataObject.CollectionChanged += (sender, args) =>
            {

                foreach (var argsNewItem in args.NewItems.Cast<TState>())
                {
                    if (args.Action == NotifyCollectionChangedAction.Add)
                        this.Add(argsNewItem);
                    else if (args.Action == NotifyCollectionChangedAction.Remove)
                        this.Remove(argsNewItem);
                }



            };
            return ObservableDataObject;
        }

        public void WatchCommand(IActionCommand command)
        {
            command.RefreshState();
            _actionCommands.Add(command);
        }

        public IDisposable Subscribe(IObserver<ObservableCollection<TState>> observer)
        {
            return _stateSubject
               .Subscribe(observer);
        }

        public ObservableCollection<TState> GetState()
        {
            return this;
        }

        private IAction InnerDispatch(IAction action)
        {

            try
            {
                lock (_syncRoot)
                {
                    Data = _reducer.Reduce(CreateObservableDataObject(), action);
                   
                }
                _stateSubject.OnNext(CreateObservableDataObject());

                foreach (var actionCommand in _actionCommands)
                {
                    actionCommand.RefreshState();
                }
            }
            catch (Exception ex)
            {
                _stateSubject.OnError(ex);
            }
            return action;
        }

    }
}