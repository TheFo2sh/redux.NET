using MVRX.Core;
using MVRX.Core.Commands;
using MVRX.Core.ViewModel;
using ReduxDotNet.Actions;
using ReduxDotNet.Model;
using ReduxDotNet.Reducers;

namespace ReduxDotNet.Features
{
    [PropertyChanged.ImplementPropertyChanged]
    public class UserManagementFeature : Feature<IStore<User, UserProfileReducer>>
    {

        [Observable]
        public User User { get; set; }

        [Action( true)]
        public ActionCommand<SetUserActiveAction> SetUserActiveAction { get; set; }

        [Action( true)]
        public ActionCommand<SetUserPendingAction> SetUserPendingAction { get; set; }
    }
}