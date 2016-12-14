using System;

namespace MVRX.Core
{
    public delegate Func<Dispatcher, Dispatcher> Middleware<TState>(IStore<TState> store);
}