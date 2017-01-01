using System.Numerics;
using MVRX.Core;
using MVRX.Core.Commands;
using MVRX.Core.ViewModel;
using ReduxDotNet.Actions;
using ReduxDotNet.Reducers;

namespace ReduxDotNet.Features
{
    [PropertyChanged.ImplementPropertyChanged]

    public class FactorialFeature : Feature<IStore<BigInteger?, FibonacciReducer>>
    {
        public int FactorialInput { get; set; }

        [Observable]
        public BigInteger? FactorialResult { get; set; }
        [Action]
        public ActionCommand<CalculateFactorialAction> CalculateFactorialAction { get; set; }
    }
}