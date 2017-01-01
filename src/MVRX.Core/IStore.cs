using System;
using System.Windows.Input;
using MVRX.Core.Commands;

namespace MVRX.Core
{
    public interface IStore<TState,TReducer> : IStore,IObservable<TState> where TReducer: Reducer<TState>
    {
        TState GetState();
    }

    public interface IStore
    {
        void VariableSubscribe(Action<object> obj);
        IAction Dispatch(IAction action);
        bool IsValid(IAction action);
        void WatchCommand(IActionCommand command);

    }
}