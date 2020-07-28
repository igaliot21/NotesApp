using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp.Model
{
    public class Notebook : INotifyPropertyChanged
    {
        private int id;
        private string name;
        private User user;
        private int userid;

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

        public User User
        {
            get { return user; }
            set { 
                user = value;
                OnPropertyChange("User");
            }
        }

        public int UserId
        {
            get { return userid; }
            set { 
                userid = value;
                OnPropertyChange("UserId");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public Notebook(){}
        public Notebook(string Name, User User) {
            this.name = Name;
            this.user = User;
            this.userid = User.Id;
        }

        private void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
