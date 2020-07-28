using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp.Model
{
    public class User : INotifyPropertyChanged
    {
        private int id;
        private string name;
        private string lastname;
        private string login;
        private string password;
        public int Id
        {
            get { return id; }
            set { 
                id = value;
                OnPropertyChange("Id");
            }
        }
        [Required]
        [MaxLength(255)]
        public string Name
        {
            get { return name; }
            set { 
                name = value;
                OnPropertyChange("Name");
            }
        }
        [Required]
        [MaxLength(255)]
        public string LastName
        {
            get { return lastname; }
            set { 
                lastname = value;
                OnPropertyChange("LastName");
            }
        }
        [Required]
        [MaxLength(32)]
        public string Login
        {
            get { return login; }
            set
            {
                login = value;
                OnPropertyChange("Login");
            }
        }
        [Required]
        [MaxLength(32)]
        public string Password
        {
            get { return password; }
            set { 
                password = value;
                OnPropertyChange("Password");
            }
        }
        public User(){}
        public User(string Name, string LastName, string Login, string Password) {
            this.name = Name;
            this.lastname = LastName;
            this.login = Login;
            this.password = Password;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}