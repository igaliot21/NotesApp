using NotesApp.Model;
using NotesApp.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp.ViewModel
{
    public class NotesVM
    {
        private NoteAppContext context;
        private Notebook selectednotebook;
        public ObservableCollection<Notebook> Notebooks { get; set; }
        public ObservableCollection<Note> Notes { get; set; }
        public NewNotebookCommand NewNotebookCmd { get; set; }
        public NewNoteCommand NewNoteCmd { get; set; }

        public Notebook SelectedNotebook
        {
            get { return selectednotebook; }
            set { 
                selectednotebook = value; 
                //TODO: get the notes
            }
        }

        public NotesVM()
        {
            context = new NoteAppContext();
            NewNotebookCmd = new NewNotebookCommand(this);
            NewNoteCmd = new NewNoteCommand(this);
        }
        public void CreateNotebook() {
            var currentUser = new User("Nombre", "Apellido", "Login", "pass"); // TODO: test only
            var newNotebook = new Notebook("New Notebook", currentUser);
            context.Notebooks.Add(newNotebook);
            context.SaveChanges();
        }
        public void CreateNote(Notebook Notebook)
        {
            var newNote = new Note("New Note", "FileLocation", Notebook);
            context.Notes.Add(newNote);
            context.SaveChanges();
        }
    }
}
