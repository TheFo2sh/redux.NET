using System.Collections.Generic;
using MVRX.Core;
using MVRX.Core.Commands;
using MVRX.Core.ViewModel;
using ReduxDotNet.Actions;
using ReduxDotNet.Reducers;

namespace ReduxDotNet.Features
{
    [PropertyChanged.ImplementPropertyChanged]
    public class SearchFeature : Feature<IStore<int?, BinarySearchReducer>>
    {
        [Action]
        public ActionCommand<SearchAction> SearchAction { get; set; }

        public Operation SearchOperation { get; set; }

        [Observable]
        public int Input { get; set; }

        public SearchFeature()
        {
            SearchOperation = new Operation(5, new List<int>() { 5, 4, 3, 2, 1 });

        }

    }
}