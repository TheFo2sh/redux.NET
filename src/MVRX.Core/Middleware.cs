using System;

namespace MVRX.Core
{
    public delegate Func<Dispatcher, Dispatcher> Middleware<TState, TReducer>(IStore<TState,TReducer> store) where TReducer : Reducer<TState>;
}