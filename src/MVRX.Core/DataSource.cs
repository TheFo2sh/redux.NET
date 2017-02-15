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
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace MVRX.Core
{
    public class DataSource<T> : ObservableCollection<T>,
        IDisposable, IDictionary<string, object>, INotifyPropertyChanged

    {

        private readonly IDictionary<string, object> _dictionaryImplementation;
        public IEnumerable<T> Data { get; set; }

        public virtual void Insert(T item)
        {
        }

        public virtual void Delete(T item)
        {
        }


        public Predicate<DataSource<T>> AvailabilityPredicate { get; set; }

        public DataSource(Predicate<DataSource<T>> availabilityPredicate, Func<IEnumerable<T>> dataFunc) :
            base(dataFunc.Invoke())
        {
            this.AvailabilityPredicate = availabilityPredicate;
            _dictionaryImplementation = new Dictionary<string, object>();
            dependentProperties = new List<string>();
            this.CollectionChanged += OnCollectionChanged;
            activeQuerys = new List<QueryObject<T>>();
            Data = this.ToList<T>();
        }

        private void OnCollectionChanged(object sender,
            NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            RefreshProperties();

            if (IsSilent)
                return;

            var newItems = notifyCollectionChangedEventArgs.NewItems?.Cast<T>()?.ToList() ?? new List<T>();
            var oldItems = notifyCollectionChangedEventArgs.OldItems?.Cast<T>()?.ToList() ?? new List<T>();

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


        private readonly List<QueryObject<T>> activeQuerys;
        public List<QueryObject<T>> Queries { get; set; }

        public void AddQuery(QueryObject<T> queryObject)
        {
            _dictionaryImplementation.Add(queryObject.GetType().Name, queryObject);
        }

        public void ExcuteFilter(QueryObject<T> queryObject)
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
                dataSource = this.ToList<T>();
                key = key.Remove(0, 8);
            }
            var property = typeof(T).GetProperty(key);
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


    }
}