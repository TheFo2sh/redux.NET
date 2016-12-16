using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using MVRX.Core.Commands;

namespace MVRX.Core
{
    public class Store<TState> : IStore<TState>
    {
        private readonly object _syncRoot = new object();
        private readonly Dispatcher _dispatcher;
        private readonly Reducer<TState> _reducer;
        private readonly ReplaySubject<TState> _stateSubject = new ReplaySubject<TState>(1);
        private TState _lastState;
        private readonly Stack<TState> _history;
        private readonly IList<IActionCommand> _actionCommands;
        public Store(Reducer<TState> reducer, TState initialState = default(TState), params Middleware<TState>[] middlewares)
        {
            _reducer = reducer;
            _dispatcher = ApplyMiddlewares(middlewares);
            _history=new Stack<TState>();
            _lastState = initialState;
            _actionCommands=new List<IActionCommand>();
            _stateSubject.OnNext(_lastState);
        }

        public IAction Dispatch(IAction action)
        {
            return _dispatcher(action);
        }

        public bool IsValid(IAction action)
        {
            var testState = _reducer.Reduce(_lastState, action);
            return _reducer.Validate(testState, action);
        }

        public void WatchCommand(IActionCommand command)
        {
            _actionCommands.Add(command);
        }

        public TState GetState()
        {
            return _lastState;
        }
        
        public IDisposable Subscribe(IObserver<TState> observer)
        {
            return _stateSubject
                .Subscribe(observer);
        }

        private Dispatcher ApplyMiddlewares(params Middleware<TState>[] middlewares)
        {
            Dispatcher dispatcher = InnerDispatch;
            foreach (var middleware in middlewares)
            {
                dispatcher = middleware(this)(dispatcher);
            }
            return dispatcher;
        }

        private IAction InnerDispatch(IAction action)
        {
           
            lock (_syncRoot)
            {
                _lastState = _reducer.Reduce(_lastState, action);
                if (!_reducer.Validate(_lastState, action))
                {
                    _lastState = _history.Pop();
                    _stateSubject.OnError( new InvalidOperationException(
                        $"Cannot apply action {action} object state will continue to be {_lastState}"));
                }
            }
            _stateSubject.OnNext(_lastState);
            _history.Push(_lastState);
            foreach (var actionCommand in _actionCommands)
            {
                actionCommand.RefreshState();
            }
            return action;
        }
    }
}
