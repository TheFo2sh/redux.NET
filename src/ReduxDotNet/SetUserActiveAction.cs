using MVRX.Core;

namespace ReduxDotNet
{
    public class SetUserActiveAction : IAction
    {
        public object Input { get; set; }
    }
}