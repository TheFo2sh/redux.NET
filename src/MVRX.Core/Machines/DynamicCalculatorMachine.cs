using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Threading.Tasks;
using Autofac.Core;
using Newtonsoft.Json.Linq;

namespace MVRX.Core.Machines
{
    public abstract class DynamicCalculatorMachine<T,P>:Reducer<P> 
    {
        private readonly Dictionary<T, P> _dictionary;
        private readonly Func<T,BigInteger> _iterations;
        private BigInteger _currentIteration;

        protected DynamicCalculatorMachine(Func<T, BigInteger> iterations):this()
        {
            this._iterations = iterations;


        }
        protected DynamicCalculatorMachine()
        {
            _dictionary = new Dictionary<T, P>();
        }

        private void AddToDictionary(T key, P value)
        {
            if (_dictionary.Count() > 1000)
                _dictionary.Clear();
            _dictionary.Add(key, value);

        }
        public void Initialize(params KeyValuePair<T, P>[] initials)
        {
            foreach (var initial in initials)
            {
                _dictionary.Add(initial.Key,initial.Value);
            }
        }
        public abstract P Divide(T input);

        public virtual P Checker(Dictionary<T, P> dictionary, T input)
        {
            if( _dictionary.ContainsKey(input))
                return _dictionary[input];
            return (P)(object)null;
        }
        public P Calculate(T input)
        {
            if (Checker(_dictionary,input) !=null)
                return Checker(_dictionary,input);

            var result = Divide(input);
           AddToDictionary(input,result);
            return result;
        }

        public override bool Validate(P previousState, P newState, IAction action)
        {
            return true;
        }

        public  void PostGuard(IAction action)
        {
            _currentIteration = _iterations.Invoke((T)action.Input);
            var bigInteger = _dictionary.Keys.Select(n => BigInteger.Parse(n.ToString())).Max();
            var iterations = BigInteger.Abs(bigInteger - _currentIteration) / 100;
            if( iterations>500)
                throw new InvalidOperationException($"Operation need {iterations} iterations while the maximum iterations number is 1000");
        }

        public override P Reduce(P previousState, IAction action)
        {
            PostGuard(action);
            _currentIteration = _iterations.Invoke((T)action.Input);
            var bigInteger = _dictionary.Keys.Select(n=>BigInteger.Parse(n.ToString())).Max();
            if (BigInteger.Abs(bigInteger - _currentIteration) > 100)
            {
                for (BigInteger j = 1; j < _currentIteration; j += 100)
                {
                    T i = (T) (object) j;
                    Calculate(i);
                }

            }
            _currentIteration = 0;
            return Calculate((T) action.Input);

        }
    }
}
