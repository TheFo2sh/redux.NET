using System;
using System.Windows.Input;
using MVRX.Core.Commands;

namespace MVRX.Core
{
    public interface IStore<TState> : IStore,IObservable<TState>
    {
        TState GetState();
    }

    public interface IStore
    {
        IAction Dispatch(IAction action);
        bool IsValid(IAction action);
        void WatchCommand(IActionCommand command);

    }
}