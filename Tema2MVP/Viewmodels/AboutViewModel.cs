using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema2MVP.Viewmodels
{
    public class AboutViewModel : BaseViewModel
    {

        private string _FirstName;
        public String FirstName
        {
            get
            {
                return _FirstName;
            }
            set
            {
                _FirstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }


        private string _Surname;
        public String Surname
        {
            get
            {
                return _Surname;
            }
            set
            {
                _Surname = value;
                OnPropertyChanged(nameof(Surname));
            }
        }

        private string _Faculty;
        public String Faculty
        {
            get
            {
                return _Faculty;
            }
            set
            {
                _Faculty = value;
                OnPropertyChanged(nameof(Faculty));
            }
        }

        private string _Specialization;
        public String Specialization
        {
            get
            {
                return _Specialization;
            }
            set
            {
                _Specialization = value;
                OnPropertyChanged(nameof(Specialization));
            }
        }


        private string _Email;
        public String Email
        {
            get
            {
                return _Email;
            }
            set
            {
                _Email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        private string _YearOfStudy;
        public String YearOfStudy
        {
            get
            {
                return _YearOfStudy;
            }
            set
            {
                _YearOfStudy = value;
                OnPropertyChanged(nameof(YearOfStudy));
            }
        }


        private string _Group;
        public String Group
        {
            get
            {
                return _Group;
            }
            set
            {
                _Group = value;
                OnPropertyChanged(nameof(Group));
            }
        }

        public AboutViewModel()
        {
            FirstName = "Leca";
            Surname = "Marian-Rafael";
            Group = "10LF293";
            Faculty = "MI";
            Specialization = "Computer Science";
            Email = "marian.leca@student.unitbv.ro";
            YearOfStudy = "II";
        }
    }
}
