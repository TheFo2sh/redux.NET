using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MVRX.Core;
using MVRX.Core.Annotations;
using MVRX.Core.Commands;
using MVRX.Core.ViewModel;
using PropertyChanged;
using ReduxDotNet.Actions;
using ReduxDotNet.Reducers;

namespace ReduxDotNet.Features
{
    public class Student
    {
        public string Name { get; set; }
        public string Grade { get; set; }

        public Student(string name,string grade)
        {
            Name = name;
            Grade = grade;
        }

        public override string ToString()
        {
            return Name;
        }
    }
    public class TextQuery : QueryObject<Student,StudentsReducer>
    {
        public TextQuery(DataSource<Student, StudentsReducer> dataSource) : base("Group1",dataSource)
        {
        }

        public override IEnumerable<Student> Query(List<Student> dataSource)
        {
            return dataSource.Where(x => x.Name.Contains(Input.ToString()));
        }
    }
    public class TextQuery2 : QueryObject<Student, StudentsReducer>
    {
        public TextQuery2(DataSource<Student, StudentsReducer> dataSource) : base("Group1",dataSource)
        {
        }

        public override IEnumerable<Student> Query(List<Student> dataSource)
        {
            return dataSource.Where(x => x.Name.EndsWith(Input.ToString()));
        }
    }
    [ImplementPropertyChanged]
    public class DataQueryFeature :  Feature<AsyncDataSource<Student, StudentsReducer>>
    {
        public AsyncDataSource<Student, StudentsReducer> Students => this.store;
        [Action(true)]
        public ActionCommand<AddToListAction> AddToListAction { get; set; }
        public DataQueryFeature()
        {
            
            this.store.AddQuery(new TextQuery(this.store));
            this.store.AddQuery(new TextQuery2(this.store));
           
        }

     
    }
}
