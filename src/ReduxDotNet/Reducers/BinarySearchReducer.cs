using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using MVRX.Core;
using MVRX.Core.Machines;

namespace ReduxDotNet.Reducers
{
    public class BinarySearchReducer : DynamicCalculatorMachine<Operation, int?>
    {
        public BinarySearchReducer() : base(n => n.InputArray.Count/2)
        {
            this.Initialize(new KeyValuePair<Operation, int?>(new Operation(),-1));
        }

        public override bool Validate(int? previousState, int? newState, IAction action)
        {
            return true;
        }

        public override Operation Partion(BigInteger partionSpace, Operation input)
        {
           return new Operation(input.Target,input.InputArray.Skip((int)partionSpace).ToList());
        }

        public override int? Checker(Dictionary<Operation, int?> dictionary, Operation input)
        {
            var isFound = dictionary.Any(x => x.Key.Equals(input));
            if (isFound)
                return dictionary.First(x => x.Key.Equals(input)).Value;
            else return null;
        }

        public override int? Divide(Operation input)
        {
            var index =(int) Math.Round(input.InputArray.Count /2f);

            var inBetwen = input.InputArray[index];
            if (inBetwen == input.Target)
                return index;
            else if (inBetwen > input.Target)
                return index+Calculate(new Operation(input.Target, input.InputArray.Skip(index).ToList()));
            else
                return index- Calculate(new Operation(input.Target, input.InputArray.Take(index).ToList()));
        }
    }
}