using System;
using System.Numerics;
using MVRX.Core;

namespace ReduxDotNet.Actions
{
    public class AddToListAction : IAction
    {
        public object Input { get; set; }
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