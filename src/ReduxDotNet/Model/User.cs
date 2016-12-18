using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.AllJoyn;
using MVRX.Core;
using Newtonsoft.Json;
using PropertyChanged;

namespace ReduxDotNet.Model
{
    public enum UserProfileStates { New, Pending, Approved }

    public class UserProfileFlow
    {
        private readonly StateFlow _stateFlow;
        public UserProfileFlow()
        {
            var newState = new State(UserProfileStates.New);
            var pendingState = new State(UserProfileStates.Pending);
            var approvedStateFlow = new State(UserProfileStates.Approved);
           // approvedStateFlow.Next = new List<State> { new State(UserProfileStates.Pending)};
            pendingState.Next=new List<State> { approvedStateFlow};
            newState.Next = new List<State> { pendingState ,approvedStateFlow};

            _stateFlow = new StateFlow(newState);
        }


        public void SetCurrentState(UserProfileStates value, bool isTimeTravel=false)
        {
            _stateFlow.SetCurrentState(value, isTimeTravel);
        }

        public bool ValidateAction(UserProfileStates previousStateProfileStates, UserProfileStates newStateProfileStates)
        {
            return _stateFlow.ValidateAction(previousStateProfileStates, newStateProfileStates);
        }
    }
   public class User
    {
        private UserProfileStates _profileStates;

        public UserProfileStates ProfileStates
        {
            get { return _profileStates; }
            set
            {
                _profileStates = value;
                ProfileState = value.ToString();
            }
        }
        public string ProfileState { get; set; }

        public User Clone()
        {
            return JsonConvert.DeserializeObject<User>(JsonConvert.SerializeObject(this));
        }
        public User()
        {
            ProfileStates=UserProfileStates.New;
        }
    }

    public class UserProfileReducer : Reducer<User>, IStateObject<UserProfileFlow>
    {
        public UserProfileFlow StateFlowManager { get; set; }
        public UserProfileReducer()
        {
            StateFlowManager=new UserProfileFlow();
        }
        public override User Reduce(User previousState, IAction action)
        {
            
            var user = previousState.Clone();
            if (action is SetUserActiveAction)
            {
                user.ProfileStates = UserProfileStates.Approved;

                return user;
            }
            if (action is SetUserPendingAction)
            {
                user.ProfileStates = UserProfileStates.Pending;

                return user;
            }

            return previousState;
        }

        public override bool Validate(User previousState, User newState, IAction action)
        {
            bool validateAction;
           
                validateAction = StateFlowManager.ValidateAction(newState.ProfileStates,  previousState.ProfileStates);
            
            return validateAction;
            
              
        }
    }
}
