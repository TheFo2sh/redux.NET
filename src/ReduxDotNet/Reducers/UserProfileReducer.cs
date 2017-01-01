using MVRX.Core;
using MVRX.Core.Machines;
using ReduxDotNet.Actions;
using ReduxDotNet.Model;

namespace ReduxDotNet.Reducers
{
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