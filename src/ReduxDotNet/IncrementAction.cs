using System.Numerics;
using MVRX.Core;

namespace ReduxDotNet
{
    public class IncrementAction : IAction
    {
        public object Input { get; set; }

        public IncrementAction(string q)
        {
            Input = int.Parse(q);
        }
    }
    public class CalculateFactorialAction : IAction
    {
        public object Input { get; set; }

        public CalculateFactorialAction(object q)
        {
            if (q == null)
                Input = (BigInteger)0;
            else
            Input = BigInteger.Parse(q.ToString());
        }
    }
}
