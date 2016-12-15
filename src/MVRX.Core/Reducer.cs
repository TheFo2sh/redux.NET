namespace MVRX.Core
{
    public interface IReducer { }
    public abstract class Reducer<TState>:IReducer
    {
        public abstract TState Reduce(TState previousState, IAction action);
        public abstract bool Validate(TState previousState, IAction action);

       
    }
}