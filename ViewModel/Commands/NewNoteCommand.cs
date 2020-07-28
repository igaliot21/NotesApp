using NotesApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NotesApp.ViewModel.Commands
{
    public class NewNoteCommand : ICommand
    {
        public NotesVM VM { get; set; }
        public event EventHandler CanExecuteChanged;
        public NewNoteCommand(NotesVM ViewModel)
        {
            this.VM = ViewModel;
        }
        public bool CanExecute(object parameter)
        {
            var selectedNotebook = parameter as Notebook;
            if (selectedNotebook != null) return true;
            else return false;
        }

        public void Execute(object parameter)
        {
            // var selectedNotebook = parameter as Notebook;
            VM.CreateNote(parameter as Notebook);
            //TODO: New Note functionality
        }
    }
}
