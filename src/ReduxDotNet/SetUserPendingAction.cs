using MVRX.Core;

namespace ReduxDotNet
{
    public class SetUserPendingAction : IAction
    {
        public object Input { get; set; }
    }
}