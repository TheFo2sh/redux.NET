using System.Collections.Generic;
using System.Numerics;
using MVRX.Core.Machines;

namespace ReduxDotNet.Reducers
{
    public class FibonacciReducer : DynamicCalculatorMachine<BigInteger, BigInteger?>
    {
        public FibonacciReducer():base(n => n)
        {
            this.Initialize(new KeyValuePair<BigInteger, BigInteger?>(0, 1),
                new KeyValuePair<BigInteger, BigInteger?>(1, 1));
        }
        public override BigInteger? Divide(BigInteger input)
        {
            return Calculate(input -1)+ Calculate(input - 1);
        }

        public override BigInteger Partion(BigInteger partionSpace, BigInteger input)
        {
            return partionSpace;
        }
    }
}