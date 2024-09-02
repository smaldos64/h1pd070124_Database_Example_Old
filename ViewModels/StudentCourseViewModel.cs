using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database_Example.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Specialized;

namespace Database_Example.ViewModels
{
    //public class StudentCourseViewModel : List<Student>, INotifyCollectionChanged
    public class StudentCourseViewModel : INotifyCollectionChanged
    {
        private Student _student_Object;
        private string _studentCourseString;

        public Student Student_Object
        {
            get
            {
                return (this._student_Object);
            }
            set
            {
                this._student_Object = value;
                //NotifyPropertyChanged("Student_Object");
            }
        }

        public string StudentCourseString
        {
            get
            {
                return (this._studentCourseString);
            }
            set
            {
                this._studentCourseString = value;
            }
        }

        public StudentCourseViewModel()
        {
            int test = 12;
        }

        public StudentCourseViewModel(Student Student_Object)
        {
            this.Student_Object = Student_Object;
        }

        public void SetCourses(Student Student_Object)
        {
            if (Student_Object.Courses.Count > 0)
            {
                this.StudentCourseString = "";

                foreach (Course Course_Object in Student_Object.Courses)
                {
                    this.StudentCourseString += Course_Object.CourseName + "\r\n";
                }
            }
            else
            {
                this.StudentCourseString = "----------";
            }
        }

        //public event PropertyChangedEventHandler PropertyChanged;
        //public void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        //{
        //    if (PropertyChanged != null)
        //    {
        //        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //    }
        //}

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        protected void OnCollectionChanged(NotifyCollectionChangedAction action)
        {
            if (this.CollectionChanged != null)
            {
                this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(action));
            }
        }
    }
}
