using MVRX.Core;
using Newtonsoft.Json;

namespace ReduxDotNet.Actions
{
    public class SearchAction : IAction
    {
        public object Input { get; set; }

        public SearchAction(object q)
        {
            if (q == null)
                Input = (Operation) new Operation();
            else if (q is Operation)
                Input = q;
            else
            {
                var value = q.ToString();
                var deserializeObject = JsonConvert.DeserializeObject<Operation>(value);
                Input = (Operation)deserializeObject;
            }
        }
    }
}