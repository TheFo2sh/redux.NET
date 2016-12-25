using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.AllJoyn;
using MVRX.Core;
using MVRX.Core.Machines;
using Newtonsoft.Json;
using PropertyChanged;

namespace ReduxDotNet.Model
{
    public enum UserProfileStates { New, Pending, Approved }

    public class UserProfileFlow: StateFlow
    {
        public UserProfileFlow():base(new State(UserProfileStates.New))
        {
            //this. = new State(UserProfileStates.New);
            var pendingState = new State(UserProfileStates.Pending);
            var approvedStateFlow = new State(UserProfileStates.Approved);
           // approvedStateFlow.Next = new List<State> { new State(UserProfileStates.Pending)};
            pendingState.Next=new List<State> { approvedStateFlow};
           this.SetNextStates( pendingState );

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

    public class UserProfileReducer : StateMachine<User, UserProfileFlow>
    {
        public UserProfileReducer() : base("ProfileStates")
        {
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


    }
}
