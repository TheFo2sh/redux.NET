using MVRX.Core;

namespace ReduxDotNet.Actions
{
    public class SetUserActiveAction : IAction
    {
        public object Input { get; set; }
    }
}