using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using Autofac;
using MVRX.Core.Commands;
using MVRX.Core.ViewModel;
using Module = Autofac.Module;

namespace MVRX.Core.Loaders
{
    public class MainLoader:Module
    {
        private readonly List<Assembly> _systemAssemblies;

        public MainLoader(params Assembly[] systemAssemblies)
        {
            _systemAssemblies = systemAssemblies.ToList();
            _systemAssemblies.Add(this.GetType().GetTypeInfo().Assembly);
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(_systemAssemblies.ToArray())
                .AsSelf();

            builder.RegisterAssemblyTypes(_systemAssemblies.ToArray())
                .AssignableTo<IStore>()
                .SingleInstance()
                .AsSelf()
                .AsImplementedInterfaces();

            builder.RegisterGeneric(typeof(Store<,>))
                .As(typeof(IStore<,>))
                .AsSelf()
                .SingleInstance();

            builder.RegisterAssemblyTypes(_systemAssemblies.ToArray())
               .AssignableTo<IReducer>()
               .As<IReducer>()
               .AsSelf()
               .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(_systemAssemblies.ToArray())
              .AssignableTo<ICommand>()
              .AsClosedTypesOf(typeof(ActionCommand<>))
              .AsImplementedInterfaces();

            builder.RegisterGeneric(typeof(ActionCommand<>))
                .AsSelf();

            builder.RegisterAssemblyTypes(_systemAssemblies.ToArray())
              .AssignableTo<IFeature>()
              .AsSelf()
              .Named<IFeature>(x=>x.Name)
              .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(_systemAssemblies.ToArray())
              .AssignableTo<BaseViewModel>()
              .AsSelf()
              .AsImplementedInterfaces()
              .PropertiesAutowired();

            base.Load(builder);
        }
    }
}
