using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using Autofac;
using MVRX.Core.Commands;
using Module = Autofac.Module;

namespace MVRX.Core.Loaders
{
    public class MainLoader:Module
    {
        private readonly Assembly[] _systemAssemblies;

        public MainLoader(params Assembly[] systemAssemblies)
        {
            _systemAssemblies = systemAssemblies;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var enumerable = _systemAssemblies.SelectMany(x => x.DefinedTypes.Select(y => y.DeclaringType));
            builder.RegisterTypes(enumerable.ToArray())
                .AssignableTo<IStore>()
                .SingleInstance()
                .AsSelf()
                .AsImplementedInterfaces();

            builder.RegisterTypes(enumerable.ToArray())
               .AssignableTo<IReducer>()
               .As<IReducer>()
               .AsImplementedInterfaces();

            builder.RegisterTypes(enumerable.ToArray())
              .AssignableTo<ICommand>()
              .AsClosedTypesOf(typeof(ActionCommand<>))
              .AsImplementedInterfaces();


            base.Load(builder);
        }
    }
}
