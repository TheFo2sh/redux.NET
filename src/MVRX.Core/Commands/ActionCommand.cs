using System;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Newtonsoft.Json;

namespace MVRX.Core.Commands
{
    public interface IActionCommand : ICommand
    {
        void RefreshState();
    }
    public class ActionCommand<T> : IActionCommand where T : IAction
    {
        public event EventHandler CanExecuteChanged;

        public void RefreshState()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged.Invoke(this, EventArgs.Empty);
        }
        private readonly IStore _store;
        public ActionCommand(IStore store)
        {
            _store = store;
            
        }

      
        public  bool CanExecute(object parameter)
        {
            return 
             _store.IsValid((CreateInstanse(JsonConvert.DeserializeObject(JsonConvert.SerializeObject(parameter)))));
        }

        public  void Execute(object parameter)
        {
            var instance = CreateInstanse(parameter);
            _store.Dispatch(instance);

        }

        private static IAction CreateInstanse(object parameter)
        {
            object instance;
            if (typeof(T).GetConstructors().Any(c=>c.GetParameters().Any()))
            {
                instance = Activator.CreateInstance(typeof(T), parameter);
            }
            else
            {
                instance = Activator.CreateInstance(typeof(T));
            }

            return (IAction) instance;
        }
    }

}
