using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;
using MVRX.Core.Commands;

namespace MVRX.Core.ViewModel
{
    internal interface IFeature { }
    public class Feature<T>:IFeature
    {
        public bool Enabled { get; set; }
        public Feature()
        {
            var store = (IStore)ServiceLocator.Current.GetInstance<T>();

            var observableProperites =
                this.GetType().GetProperties().Where(prop => prop.GetCustomAttribute<ObservableAttribute>() != null);
            foreach (var observableProperty in observableProperites)
            {
                store.VariableSubscribe(x => observableProperty.SetMethod.Invoke(this, new[] { x }));
            }
            var actions = this.GetType().GetProperties().Where(prop => prop.GetCustomAttribute<ActionAttribute>() != null);
            foreach (var action in actions)
            {
                var isWatched = action.GetCustomAttribute<ActionAttribute>().IsWatched;

                var actionCommandInstanse = Activator.CreateInstance(action.PropertyType, new object[] { store });
                this.GetType()
                    .GetProperty(action.Name)
                    .SetValue(this, actionCommandInstanse);
                if (isWatched)
                    store.WatchCommand((IActionCommand)actionCommandInstanse);

            }
        }
    }
}
