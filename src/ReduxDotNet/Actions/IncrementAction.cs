using MVRX.Core;

namespace ReduxDotNet.Actions
{
    public class IncrementAction : IAction
    {
        public object Input { get; set; }

        public IncrementAction(string q)
        {
            Input = int.Parse(q);
        }
    }
}
