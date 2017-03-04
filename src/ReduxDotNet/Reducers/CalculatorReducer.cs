using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MVRX.Core;
using ReduxDotNet.Actions;
using ReduxDotNet.Features;

namespace ReduxDotNet.Reducers
{
    public class StudentsReducer : Reducer<ObservableCollection<Student>>
    {
        public override ObservableCollection<Student> Reduce(ObservableCollection<Student> previousState, IAction action)
        {
            if (action is AddToListAction)
            {
                previousState.Add(new Student("Onss", "Q"));
            }
            return previousState;

        }

        public override bool Validate(ObservableCollection<Student> previousState, ObservableCollection<Student> newState, IAction action)
        {
            return newState.Count<6;
        }
    }
    public class CalculatorReducer:Reducer<int>
    {
       

        public override int Reduce(int previousState, IAction action)
        {
            var state = previousState;
            if (action is IncrementAction)
            {
                return state + (int)((IncrementAction)action).Input;
            }
            if (action is DecrementAction)
            {
                return state - 1;
            }
            return state;
        }

        public override bool Validate(int nextState, int previousState, IAction action)
        {
            return previousState < 15;
        }
    }
}
