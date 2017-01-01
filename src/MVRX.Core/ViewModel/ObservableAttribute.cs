using System;

namespace MVRX.Core.ViewModel
{
    public class ActionAttribute : Attribute
    {
        public bool IsWatched { get; set; }
        public ActionAttribute(bool isWatched=false)
        {
            IsWatched = isWatched;
        }
    }
    public class ObservableAttribute : Attribute
    {

    }
}