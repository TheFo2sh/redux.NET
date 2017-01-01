using MVRX.Core;

namespace ReduxDotNet.Actions
{
    public class SetUserPendingAction : IAction
    {
        public object Input { get; set; }
    }
}