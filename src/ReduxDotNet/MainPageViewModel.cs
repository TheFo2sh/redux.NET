using System;
using System.Diagnostics;
using MVRX.Core.Commands;
using ReduxDotNet.Model;

namespace ReduxDotNet
{
    [PropertyChanged.ImplementPropertyChanged]
    public class MainPageViewModel
    {
        public int Count { get; set; }
        public string UserStatus { get; set; }

        public ActionCommand<IncrementAction> IncrementAction { get; set; }
        public ActionCommand<DecrementAction> DecrementAction { get; set; }

        public ActionCommand<SetUserActiveAction> SetUserActiveAction { get; set; }
        public ActionCommand<SetUserPendingAction> SetUserPendingAction { get; set; }

        public MainPageViewModel()
        {
            IncrementAction = new ActionCommand<IncrementAction>(App.CounterStore);
            DecrementAction =new ActionCommand<DecrementAction>(App.CounterStore);

            SetUserActiveAction = new ActionCommand<SetUserActiveAction>(App.UserStore);
            SetUserPendingAction = new ActionCommand<SetUserPendingAction>(App.UserStore);
            //            App.CounterStore.Subscribe(i => IncrementAction.RefreshState());
            UserStatus = App.UserStore.GetState().ProfileState;
            App.CounterStore.WatchCommand(IncrementAction);
            App.CounterStore.Subscribe(
                counter => Count = counter, (exception => { Debug.Write(exception.Message); }));

            App.UserStore.WatchCommand(SetUserActiveAction);
            App.UserStore.WatchCommand(SetUserPendingAction);

            App.UserStore.Subscribe(user =>
            {
                UserStatus = user.ProfileState;
            }, (exception => { Debug.Write(exception.Message); }));
        }
    }
}