using MVRX.Core;
using ReduxDotNet.Actions;

namespace ReduxDotNet.Reducers
{
    public class CalculatorReducer:Reducer<int>
    {
       

        public override int Reduce(int previousState, IAction action)
        {
            var state = previousState;
            if (action is IncrementAction)
            {
                return state + (int)((IncrementAction)action).Input;
            }
            if (action is DecrementAction)
            {
                return state - 1;
            }
            return state;
        }

        public override bool Validate(int nextState, int previousState, IAction action)
        {
            return previousState < 15;
        }
    }
}
