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
            get { return user; }
            set { user = value; }
        }
        public RegisterCommand RegisterCmd { get; set; }
        public LoginCommand LoginCmd { get; set; }
        public LoginVM()
        {
            context = new NoteAppContext();
            RegisterCmd = new RegisterCommand(this);
            LoginCmd = new LoginCommand(this);
        }
    }
}
