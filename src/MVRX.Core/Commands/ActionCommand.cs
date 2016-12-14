using System;
using System.Windows.Input;

namespace MVRX.Core.Commands
{
    public class ActionCommand<T> : ICommand where T : IAction
    {
        public event EventHandler CanExecuteChanged;
        private readonly IStore _store;

        public ActionCommand(IStore store)
        {
            _store = store;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter != null)
            {
                var instance = Activator.CreateInstance(typeof(T), parameter);
                _store.Dispatch((IAction)instance);
            }
            else
            {
                var instance = Activator.CreateInstance(typeof(T));
                _store.Dispatch((IAction)instance);
            }
        }
    }

}
