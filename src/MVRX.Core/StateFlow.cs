using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MVRX.Core
{
    public interface IStateObject<T>
    {
      T StateFlowManager { get; set; } 
    }

    public class State
    {
        private Enum _currentState;

        public Enum GetState()
        {
            return _currentState;
        }
        public State(Enum currentState)
        {
            _currentState = currentState;
        }
        public List<State> Next { get; set; }
        public bool IsSelected { get; set; }
    }
    public class StateFlow 
    {
        private List<State> StateHistory;
        private State CurrentState { get; set; }

        private readonly State _root;
        public StateFlow(State state)
        {
            StateHistory=new List<State>();
            CurrentState = state;
            _root = state;
        }

        public void SetCurrentState(Enum state,bool isTimeTravel=false)
        {
            CurrentState =  traverse(new List<State>() { CurrentState },state);
          
        }

        private State traverse(List<State> root, Enum state)
        {
            foreach (var cursor in root)
            {
                StateHistory.Add(cursor);

                if (Equals(cursor.GetState(), state))
                    return cursor;

                if(cursor.Next==null)
                    continue;

                foreach (var item in cursor.Next)
                {
                    StateHistory.Add(item);

                    if (Equals(item.GetState(), state))
                        return item;
                    var innerresult= traverse(cursor.Next, state);
                    if (innerresult != null)
                        return innerresult;
                }
            }
            return null;
        }

        public Enum GetState()
        {
            return CurrentState.GetState();
        }

        
        public void SetNextStates(params State[] state)
        {
            CurrentState.Next = state.ToList();
        }
        public bool ValidateAction(Enum nextState, Enum previousState)
        {
            SetCurrentState(previousState);
            StateHistory.Clear();
            SetCurrentState(nextState);
            var numberOfStepsToHaveDirectConnection = 2;
            try
            {
              
                return StateHistory.Count == numberOfStepsToHaveDirectConnection;
            }
            finally
            {
                StateHistory.Clear();
                CurrentState = _root;
            }
        }
    }
}
