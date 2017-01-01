using System;
using System.Diagnostics;
using MVRX.Core.Commands;
using MVRX.Core.ViewModel;
using ReduxDotNet.Actions;
using ReduxDotNet.Features;

namespace ReduxDotNet
{
    //[PropertyChanged.ImplementPropertyChanged]
    public class MainPageViewModel: BaseViewModel
    {
        public UserManagementFeature UserManagementFeature { get; set; }
        public CalculatorFeature CalculatorFeature { get; set; }
        public SearchFeature SearchFeature { get; set; }
        public FactorialFeature FactorialFeature { get; set; }

    }
}