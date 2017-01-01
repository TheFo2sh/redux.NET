using MVRX.Core;
using MVRX.Core.Commands;
using MVRX.Core.ViewModel;
using ReduxDotNet.Actions;
using ReduxDotNet.Reducers;

namespace ReduxDotNet.Features
{
    [PropertyChanged.ImplementPropertyChanged]
    public class CalculatorFeature : Feature<IStore<int, CalculatorReducer>>
    {

        [Action( true)]
        public ActionCommand<IncrementAction> IncrementAction { get; set; }
        [Action( true)]
        public ActionCommand<DecrementAction> DecrementAction { get; set; }

        [Observable]
        public int Count { get; set; }
    }
}