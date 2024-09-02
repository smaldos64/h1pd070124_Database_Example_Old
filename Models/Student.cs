using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Database_Example.Models
{
    public class Student : INotifyPropertyChanged
    {
        public int StudentID { get; set; }

        //public string StudentName { get; set; }

        private string _studentName;
        public string StudentName
        {
            get
            {
                return (this._studentName);
            }
            set
            {
                this._studentName = value;
                NotifyPropertyChanged("StudentName");
            }
        }

        public string StudentLastName { get; set; }

        public int TeamID { get; set; }
        public virtual Team Team { get; set; }

        //public string PictureName { get; set; }

        public virtual List<Course> Courses { get; set; }

        //public virtual List<Picture> Pictures { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
