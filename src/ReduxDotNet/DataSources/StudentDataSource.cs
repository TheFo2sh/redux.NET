using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using MVRX.Core;
using ReduxDotNet.Features;
using ReduxDotNet.Reducers;

namespace ReduxDotNet.DataSources
{
    public class StudentDataSource : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(context =>
            {
                return new AsyncDataSource<Student, StudentsReducer>(source => true,
                    Task.Run(() => 
                    new List<Student>()
                    {
                        new Student("Ahmed", "A"),
                        new Student("Kmar", "A++"),
                        new Student("Amr", "C")
                    }.AsEnumerable())
                );
            });
        }
    }
}
