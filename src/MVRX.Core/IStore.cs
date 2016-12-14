using System;

namespace MVRX.Core
{
    public interface IStore<TState> : IStore,IObservable<TState>
    {
        TState GetState();
    }

    public interface IStore
    {
        IAction Dispatch(IAction action);

    }
}