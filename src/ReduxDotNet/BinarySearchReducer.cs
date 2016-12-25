using System;
using System.Collections.Generic;
using System.Linq;
using MVRX.Core;
using MVRX.Core.Machines;

namespace ReduxDotNet
{
    public class Operation
    {
        public int Target { get; set; }
        public List<int> InputArray { get; set; }

        public Operation()
        {
            Target=new int();
            InputArray=new List<int>();
        }

        public override string ToString()
        {
            return InputArray.Count.ToString();
        }
    }
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

        public override int? Checker(Dictionary<Operation, int?> dictionary, Operation input)
        {
            var isFound = dictionary.Any(x => x.Key.InputArray.Count() == input.InputArray.Count());
            if (isFound)
                return dictionary.First(x => x.Key.InputArray.Count() == input.InputArray.Count()).Value;
            else return null;
        }

        public override int? Divide(Operation input)
        {
            var index =(int) Math.Round(input.InputArray.Count /2f);

            var inBetwen = input.InputArray[index];
            if (inBetwen == input.Target)
                return index;
            else if (inBetwen > input.Target)
                return Calculate(new Operation()
                {
                    InputArray = input.InputArray.Skip(index).ToList(),
                    Target = input.Target
                });
            else
                return Calculate(new Operation()
                {
                    InputArray = input.InputArray.Take(index).ToList(),
                    Target = input.Target
                });
        }
    }
}