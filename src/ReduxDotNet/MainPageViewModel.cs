using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using MVRX.Core.Commands;
using ReduxDotNet.Model;

namespace ReduxDotNet
{
    [PropertyChanged.ImplementPropertyChanged]
    public class MainPageViewModel
    {
        public int Count { get; set; }
        public BigInteger FactorialResult { get; set; }
        public int FactorialInput { get; set; }
        public string UserStatus { get; set; }

        public ActionCommand<IncrementAction> IncrementAction { get; set; }
        public ActionCommand<DecrementAction> DecrementAction { get; set; }

        public ActionCommand<SetUserActiveAction> SetUserActiveAction { get; set; }
        public ActionCommand<SetUserPendingAction> SetUserPendingAction { get; set; }
        public ActionCommand<CalculateFactorialAction> CalculateFactorialAction { get; set; }
        public ActionCommand<SearchAction> SearchAction { get; set; }

        public Operation SearchOperation { get; set; }
        public MainPageViewModel()
        {
            IncrementAction = new ActionCommand<IncrementAction>(App.CounterStore);
            DecrementAction =new ActionCommand<DecrementAction>(App.CounterStore);
            SearchAction = new ActionCommand<SearchAction>(App.BinarySearchStore);
            CalculateFactorialAction =new ActionCommand<CalculateFactorialAction>(App.FactorialStore);
            SetUserActiveAction = new ActionCommand<SetUserActiveAction>(App.UserStore);
            SetUserPendingAction = new ActionCommand<SetUserPendingAction>(App.UserStore);
            SearchOperation = new Operation() {InputArray = new List<int>() {5, 4, 3, 2, 1},Target = 3};
                        App.CounterStore.Subscribe(i => IncrementAction.RefreshState());
            UserStatus = App.UserStore.GetState().ProfileState;
            App.CounterStore.WatchCommand(IncrementAction);
            App.CounterStore.Subscribe(
                counter => Count = counter, (exception => { Debug.Write(exception.Message); }));
            App.BinarySearchStore.Subscribe(
                (i) =>
                {
                    FactorialInput = i.Value;
                }, 
                (exception =>
                {
                    Debug.Write(exception.Message);
                }));
            App.UserStore.WatchCommand(SetUserActiveAction);
            App.UserStore.WatchCommand(SetUserPendingAction);
            App.FactorialStore.Subscribe(i => FactorialResult = i.Value,
                exception =>
                {
                    Debug.Write(exception.Message);
                });
            App.UserStore.Subscribe(user =>
            {
                UserStatus = user.ProfileState;
            }, (exception => { Debug.Write(exception.Message); }));
        }
    }
}