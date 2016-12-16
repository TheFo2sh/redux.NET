using MVRX.Core;

namespace ReduxDotNet
{
    public class IncrementAction : IAction
    {
        public int Quantity { get; set; }

        public IncrementAction(string q)
        {
            Quantity = int.Parse(q);
        }
    }
}
