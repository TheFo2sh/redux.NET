using MVRX.Core;

namespace ReduxDotNet
{
    public class CalculatorReducer:Reducer<int>
    {
       

        public override int Reduce(int previousState, IAction action)
        {
            var state = previousState;
            if (action is IncrementAction)
            {
                return state + ((IncrementAction)action).Quantity;
            }
            if (action is DecrementAction)
            {
                return state - 1;
            }
            return state;
        }

        public override bool Validate(int previousState, IAction action)
        {
            return previousState < 15;
        }
    }
}
