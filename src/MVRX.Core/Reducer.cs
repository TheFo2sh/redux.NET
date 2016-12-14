namespace MVRX.Core
{
    public delegate TState Reducer<TState>(TState previousState, IAction action);
}