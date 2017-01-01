using System.Collections.Generic;

namespace ReduxDotNet
{
    public class Operation
    {
        public  int Target { get; set; }
        public  List<int> InputArray { get; set; }
        public override bool Equals(object obj)
        {
            var target =  obj as Operation;
            if (target == null)
                return false;
            if (InputArray.Count == 0 && target.InputArray.Count == 0)
                return true;
            if (this.GetHashCode()==obj.GetHashCode())
                return true;
            return false;
        }

        public override int GetHashCode()
        {
            if (InputArray.Count == 0)
                return Target;
            return int.Parse(string.Join("", InputArray)+InputArray.Count+ Target) ;
        }

        public Operation()
        {
            Target=new int();
            InputArray=new List<int>();
        }
        public Operation(int target, List<int> inputArray)
        {
            Target = target;
            InputArray = inputArray;
        }
        public override string ToString()
        {
            return InputArray.Count.ToString();
        }
    }
}