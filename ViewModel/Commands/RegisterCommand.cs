﻿using NotesApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NotesApp.ViewModel.Commands
{
    public class RegisterCommand : ICommand
    {
        public LoginVM VM { get; set; }

        public event EventHandler CanExecuteChanged;
        public RegisterCommand(LoginVM ViewModel)
        {
            this.VM = ViewModel;
        }
        public bool CanExecute(object parameter)
        {
            /*
            var user = parameter as User;
            if (user != null){
                if (string.IsNullOrEmpty(user.Login)) return false;
                else if (string.IsNullOrEmpty(user.Password)) return false;
                else if (string.IsNullOrEmpty(user.Name)) return false;
                else if (string.IsNullOrEmpty(user.LastName)) return false;
                else return true;
            }
            else return false;
            */
            return true;
        }

        public void Execute(object parameter)
        {
            var user = parameter as User;
            if (user != null)
            {
                if (!string.IsNullOrEmpty(user.Login) &&
                    !string.IsNullOrEmpty(user.Password) &&
                    !string.IsNullOrEmpty(user.Name) &&
                    !string.IsNullOrEmpty(user.LastName)) 
                    VM.Register();
            }
        }
    }
}
