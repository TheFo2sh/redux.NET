using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MVRX.Core.Machines
{
    public abstract class StateMachine<T,TP>:Reducer<T>,IStateObject<TP> where TP : StateFlow, new()
    {
        private string _targetProperty;

        protected StateMachine(string targetProperty)
        {
            _targetProperty = targetProperty;
            StateFlowManager=new TP();
        }

        public abstract override T Reduce(T previousState, IAction action);
        

        public override bool Validate(T previousState, T newState, IAction action)
        {
            return StateFlowManager.ValidateAction(GetPropertyFromObject(newState), GetPropertyFromObject(previousState));
        }

        private Enum GetPropertyFromObject(object obj)
        {
           return (Enum) obj.GetType().GetProperty(_targetProperty).GetValue(obj);
        }
        public TP StateFlowManager { get; set; }
    }
}
