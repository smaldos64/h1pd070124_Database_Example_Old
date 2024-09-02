using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Database_Example.Models;
using Database_Example.Windows;
using Database_Example.Settings;
using Database_Example.ViewModels;
using System.Collections.ObjectModel;

using Database_Example.ExtensionMethods;

namespace Database_Example
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        private DatabaseContext db = new DatabaseContext();
        //private List<Student> StudentList = new List<Student>();
        //private ObservableCollection<Student> StudentList = new ObservableCollection<Student>();
        private ObservableCollection<Student> StudentList;
        public static List<ProjectSettings> SettingsList = new List<ProjectSettings>();

        public MainWindow()
        {
            //this.DataContext = new StudentCourseViewModel();
            
            InitializeComponent();
            //dataGrid.DataContext = new StudentCourseViewModel();
            dataGrid.DataContext = StudentList = new ObservableCollection<Student>();
            //dataGrid.ItemsSource = StudentList;

            BindStudentList();
            SetupWebApiUrls();
        }

        private void SetupWebApiUrls()
        {
            SettingsList.Clear();
            SettingsList.Add(new ProjectSettings(WEB_API_CONTROLLER_ENUM.STUDENT_API_CONTROLLER, 
                             Properties.Settings.Default.WEB_API_STUDENT_URL));
            SettingsList.Add(new ProjectSettings(WEB_API_CONTROLLER_ENUM.TEAM_API_CONTROLLER,
                             Properties.Settings.Default.WEB_API_TEAM_URL));
            SettingsList.Add(new ProjectSettings(WEB_API_CONTROLLER_ENUM.COURSE_API_CONTROLLER,
                             Properties.Settings.Default.WEB_API_COURSE_URL));
        }

        public static string Find_WEB_API_URL(WEB_API_CONTROLLER_ENUM This_Web_Api_Controller_Enum)
        {
            return (SettingsList.First(item => item.Web_Api_Controller_Enum == This_Web_Api_Controller_Enum).Web_Api_Controller_Url);
        }

        private void BindStudentList()
        {
            DatabaseContext db1 = new DatabaseContext();
            //dataGrid.Items.Clear();
            
            StudentList = ObservableCollectionExtensions.ToObservableCollection<Student>(db1.Students.ToList());
            //StudentList = db1.Students.ToList() as ObservableCollection<Student>;

#if No_Database_Present
            Team TeamObject = new Team();
            TeamObject.TeamID = 1;
            TeamObject.TeamName = "Team 1";
            TeamObject.Students = new List<Student>();

            Team TeamObject1 = new Team();
            TeamObject1.TeamID = 2;
            TeamObject1.TeamName = "Team 2";
            TeamObject1.Students = new List<Student>();

            Student StudentObject = new Student();
            StudentObject.StudentID = 1;
            StudentObject.StudentName = "Lars";
            StudentObject.StudentLastName = "Pedersen";
            StudentObject.TeamID = TeamObject.TeamID;
            StudentObject.Team = new Team();
            StudentObject.Team.TeamName = TeamObject.TeamName;
            StudentObject.Courses = new List<Course>();

            Student StudentObject1 = new Student();
            StudentObject1.StudentID = 2;
            StudentObject1.StudentName = "Jette";
            StudentObject1.TeamID = TeamObject1.TeamID;
            StudentObject1.Courses = new List<Course>();

            Course CourseObject = new Course();
            CourseObject.CourseID = 1;
            CourseObject.CourseName = "Matematik";
            CourseObject.Students = new List<Student>();

            Course CourseObject1 = new Course();
            CourseObject1.CourseID = 2;
            CourseObject1.CourseName = "Fysik";
            CourseObject1.Students = new List<Student>();

            Course CourseObject2 = new Course();
            CourseObject2.CourseID = 3;
            CourseObject2.CourseName = "Kemi";
            CourseObject2.Students = new List<Student>();

            StudentObject.Courses.Add(CourseObject);
            StudentObject.Courses.Add(CourseObject2);

            StudentObject1.Courses.Add(CourseObject1);
            StudentObject1.Courses.Add(CourseObject2);

            TeamObject.Students.Add(StudentObject);
            TeamObject1.Students.Add(StudentObject1);

            StudentList.Add(StudentObject);
            StudentList.Add(StudentObject1);
#endif

            foreach (Student Student_Object in StudentList)
            {
                StudentCourseViewModel StudentCourseMethodWindow_Object = new StudentCourseViewModel(Student_Object);

                StudentCourseMethodWindow_Object.SetCourses(Student_Object);

                dataGrid.Items.Add(StudentCourseMethodWindow_Object);
            }

            //dataGrid.ItemsSource = StudentList;
        }

        private void btnEraseStudent_Click(object sender, RoutedEventArgs e)
        {
            Button ThisButon = sender as Button;
            int StudentID = Convert.ToInt32(ThisButon.Content);

            Student Student_Object = db.Students.Find(StudentID);
            //Student Student_Object = StudentList.Find(s =>  s.StudentID == StudentID);

            MessageBoxResult Result = MessageBox.Show("Ønsker du virkelig at slette eleven " + Student_Object.StudentName, "Slet Elev ?", MessageBoxButton.OKCancel);

            if (MessageBoxResult.OK == Result)
            {
                db.Students.Remove(Student_Object);
                db.SaveChanges();
                //BindStudentList();

                //Student Student_Object_In_List = StudentList.Find(s => s.StudentID == StudentID);
                Student Student_Object_In_List = StudentList.Single(s => s.StudentID == StudentID);
                StudentList.Remove(Student_Object_In_List);
            }
        }

        //public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> enumerable)
        //{
        //    return new ObservableCollection<T>(enumerable);
        //}

        private void btnModifyStudent_Click(object sender, RoutedEventArgs e)
        {
            int IndexInlist;

            Button ThisButon = sender as Button;
            int StudentID = Convert.ToInt32(ThisButon.Content);
            //Student Student_Object_In_List = StudentList.Find(s => s.StudentID == StudentID);
            Student Student_Object_In_List = StudentList.Single(s => s.StudentID == StudentID);

            // Skift til ModifyStudentWindow vindue/view
            ModifyStudentWindow dlg = new ModifyStudentWindow(StudentID);
            //ModifyStudentWindow dlg = new ModifyStudentWindow(ref Student_Object_In_List);

            dlg.ShowDialog();

            IndexInlist = StudentList.FindIndex(s => s.StudentID == StudentID);
            if (-1 != IndexInlist)
            {
                //StudentList[IndexInlist] = dlg.Student_Object;
                StudentList[IndexInlist].StudentName = dlg.Student_Object.StudentName;
            }
            //StudentList[0].StudentName = "Edited";

            //Student_Object_In_List = dlg.Student_Object;
            //BindStudentList();
        }

        private void btnNewStudent_Click(object sender, RoutedEventArgs e)
        {
            AddStudentWindow dlg = new AddStudentWindow();
            dlg.ShowDialog();

            BindStudentList();
        }

        private void btnjSonMode_Click(object sender, RoutedEventArgs e)
        {
            jSonStudentList dlg = new jSonStudentList();
            dlg.ShowDialog();
        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            StudentList[0].StudentName = "Test";    
        }
    }
}
