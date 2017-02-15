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
using MVRX.Core.ViewModel;
using PropertyChanged;

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
    public class TextQuery : QueryObject<Student>
    {
        public TextQuery(DataSource<Student> dataSource) : base("Group1",dataSource)
        {
        }

        public override IEnumerable<Student> Query(List<Student> dataSource)
        {
            return dataSource.Where(x => x.Name.Contains(Input.ToString()));
        }
    }
    public class TextQuery2 : QueryObject<Student>
    {
        public TextQuery2(DataSource<Student> dataSource) : base("Group2",dataSource)
        {
        }

        public override IEnumerable<Student> Query(List<Student> dataSource)
        {
            return dataSource.Where(x => x.Name.EndsWith(Input.ToString()));
        }
    }
    [ImplementPropertyChanged]
    public class DataQueryFeature :  IFeature
    {
        public DataSource<Student> Students { get; set; }

        public ICommand AddCommand => new RelayCommand(() => Students.Add(new Student("Ons", "P")));

        public DataQueryFeature()
        {

            Students = new DataSource<Student>(source => true,
                () => new List<Student>() {
                    new Student("Ahmed","A"),
                    new Student("Kmar","A++"),
                    new Student("Amr","C")});
            Students.AddQuery(new TextQuery(Students));
            Students.AddQuery(new TextQuery2(Students));
           
        }

     
    }
}
