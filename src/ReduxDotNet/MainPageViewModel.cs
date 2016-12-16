using System;
using System.Diagnostics;
using MVRX.Core.Commands;

namespace ReduxDotNet
{
    [PropertyChanged.ImplementPropertyChanged]
    public class MainPageViewModel
    {
        public int Count { get; set; }
        public ActionCommand<IncrementAction> IncrementAction { get; set; }
        public ActionCommand<DecrementAction> DecrementAction { get; set; }

        public MainPageViewModel()
        {
            IncrementAction = new ActionCommand<IncrementAction>(App.CounterStore);
            DecrementAction =new ActionCommand<DecrementAction>(App.CounterStore);
            //            App.CounterStore.Subscribe(i => IncrementAction.RefreshState());

            App.CounterStore.WatchCommand(IncrementAction);
            App.CounterStore.Subscribe(
                counter => Count = counter, (exception => { Debug.Write(exception.Message); }));
        }
    }
}