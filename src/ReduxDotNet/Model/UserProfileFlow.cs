using System.Collections.Generic;
using MVRX.Core;

namespace ReduxDotNet.Model
{
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
}