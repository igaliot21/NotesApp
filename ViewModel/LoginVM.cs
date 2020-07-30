using NotesApp.Model;
using NotesApp.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp.ViewModel
{
    public class LoginVM
    {
        NoteAppContext context;
        private User user;
        public User User
        {
            get { return this.user; }
            set { this.user = value; }
        }
        public RegisterCommand RegisterCommand { get; set; }
        public LoginCommand LoginCommand { get; set; }

        public event EventHandler HasLoggedIn;
        public LoginVM()
        {
            context = new NoteAppContext();
            User = new User();
            RegisterCommand = new RegisterCommand(this);
            LoginCommand = new LoginCommand(this);
        }
        public void Login() {
            var loggedUser = context.Users.Where(u => u.Login == this.user.Login).FirstOrDefault();
            if (loggedUser == null) { /*mensaje de error */ }
            else {
                if (this.user.Password == loggedUser.Password)
                {
                    App.UserId = loggedUser.Id;
                    HasLoggedIn(this, new EventArgs());
                }
                else { /*mensaje de error */ }
            }
        }
        public void Register() {
            context.Users.Add(this.User);
            context.SaveChanges();
            App.UserId = this.user.Id;
            HasLoggedIn(this, new EventArgs());
        }

    }
}
