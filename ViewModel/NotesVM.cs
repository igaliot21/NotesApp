using NotesApp.Model;
using NotesApp.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Peers;

namespace NotesApp.ViewModel
{
    public class NotesVM
    {
        private NoteAppContext context;

        public ObservableCollection<Notebook> Notebooks { get; set; }

        private Notebook selectedNotebook;
        public Notebook SelectedNotebook
        {
            get { return selectedNotebook; }
            set
            {
                selectedNotebook = value;
                ReadNotes();
            }
        }

        public ObservableCollection<Note> Notes { get; set; }

        public NewNotebookCommand NewNotebookCmd { get; set; }
        public NewNoteCommand NewNoteCmd { get; set; }

        public NotesVM(){
            context = new NoteAppContext();
            NewNotebookCmd = new NewNotebookCommand(this);
            NewNoteCmd = new NewNoteCommand(this);
            Notebooks = new ObservableCollection<Notebook>();
            Notes = new ObservableCollection<Note>();
            ReadNotebooks();
            ReadNotes();
        }
        public void CreateNotebook(){
            var currentUser = context.Users.Where(u => u.Id == 1).First(); //new User("Nombre", "Apellido", "Login", "pass"); // test only
            var newNotebook = new Notebook("New Notebook", currentUser);
            context.Notebooks.Add(newNotebook);
            context.SaveChanges();
            ReadNotebooks();
        }
        public void CreateNote(Notebook Notebook){
            var newNote = new Note("New Note", "FileLocation", Notebook);
            context.Notes.Add(newNote);
            context.SaveChanges();
            ReadNotes();
        }
        public void ReadNotebooks(){
            var notebooksRetrieved = context.Notebooks.Where(n => n.UserId == 1).ToList(); // test only
            this.Notebooks.Clear();
            foreach (var item in notebooksRetrieved) this.Notebooks.Add(item);
        }
        public void ReadNotes() {
            if (SelectedNotebook != null){
                var notesRetrieved = context.Notes.Where(n => n.NotebookId == SelectedNotebook.Id).ToList();
                this.Notes.Clear();
                foreach (var item in notesRetrieved) this.Notes.Add(item);
            }
        }
    }
}
