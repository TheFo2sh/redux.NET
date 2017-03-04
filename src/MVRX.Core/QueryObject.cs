using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MVRX.Core
{
    public abstract class QueryObject<T,TP>:ICommand where TP : Reducer<ObservableCollection<T>>
    {
        private readonly DataSource<T,TP> _dataSource;
        private  object _input;
        public string GroupName { get; private set; }

        public object Input
        {
            get { return _input; }
            set
            {
                _input = value;
                RaisePropertyChanged();
            }
        }

        private void RaisePropertyChanged()
        {
            _dataSource.ExcuteFilter(this);
        }

        public abstract IEnumerable<T> Query( List<T> dataSource);
        protected QueryObject(string groupname,DataSource<T,TP> dataSource)
        {
            GroupName = groupname;
            _dataSource = dataSource;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Input = parameter;
        }

        public event EventHandler CanExecuteChanged;
    }
}