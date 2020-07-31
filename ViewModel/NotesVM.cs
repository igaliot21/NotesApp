using NotesApp.Model;
using NotesApp.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Peers;

namespace NotesApp.ViewModel
{
    public class NotesVM : INotifyPropertyChanged 
    {
        private NoteAppContext context;
        private string username;
        private bool isediting;
        private Notebook selectedNotebook;
        private Note selectedNote;

        public string UserName
        {
            get { return username; }
            set {
                username = value;
                ReadNotebooks();
            }
        }
        public bool IsEditing {
            get { return isediting; }
            set {
                isediting = value;
                OnPropertyChange("IsEditing");
            } 
        }
        public Notebook SelectedNotebook
        {
            get { return selectedNotebook; }
            set
            {
                selectedNotebook = value;
                ReadNotes();
            }
        }
        public Note SelectedNote
        {
            get { return selectedNote; }
            set
            {
                selectedNote = value;
                SelectedNoteChanged(this, new EventArgs());
            }
        }

        public ObservableCollection<Notebook> Notebooks { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler SelectedNoteChanged;

        public ObservableCollection<Note> Notes { get; set; }
        public NewNotebookCommand NewNotebookCmd { get; set; }
        public NewNoteCommand NewNoteCmd { get; set; }
        public BeginEditCommand BeginEditCmd { get; set; }
        public HasEditedCommand HasEditedCmd { get; set; }

        public NotesVM(){
            IsEditing = false;
            context = new NoteAppContext();
            NewNotebookCmd = new NewNotebookCommand(this);
            NewNoteCmd = new NewNoteCommand(this);
            BeginEditCmd = new BeginEditCommand(this);
            HasEditedCmd = new HasEditedCommand(this);
            Notebooks = new ObservableCollection<Notebook>();
            Notes = new ObservableCollection<Note>();
            ReadNotebooks();
            ReadNotes();
        }
        private void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public void CreateNotebook(){
            if (App.UserId != 0)
            {
                var currentUser = context.Users.Where(u => u.Id == App.UserId).First(); //new User("Nombre", "Apellido", "Login", "pass"); // test only
                var newNotebook = new Notebook("New Notebook", currentUser);
                context.Notebooks.Add(newNotebook);
                context.SaveChanges();
                ReadNotebooks();
            }
        }
        public void CreateNote(Notebook Notebook){
            var newNote = new Note("New Note", "FileLocation", Notebook);
            context.Notes.Add(newNote);
            context.SaveChanges();
            ReadNotes();
        }
        public void ReadNotebooks(){
            if (App.UserId != 0)
            {
                var notebooksRetrieved = context.Notebooks.Where(n => n.UserId == App.UserId).ToList();
                this.Notebooks.Clear();
                foreach (var item in notebooksRetrieved) this.Notebooks.Add(item);
            }
            else this.Notebooks.Clear();

        }
        public void ReadNotes() {
            if (SelectedNotebook != null){
                var notesRetrieved = context.Notes.Where(n => n.NotebookId == SelectedNotebook.Id).ToList();
                this.Notes.Clear();
                foreach (var item in notesRetrieved) this.Notes.Add(item);
            }
        }
        public void StartEditing() {
            IsEditing = true;
        }
        public void HasRenamed(Notebook notebook) { 
            if(notebook != null){
                var notebookToedit = context.Notebooks.Where(n => n.Id == notebook.Id).Single();
                notebookToedit.Name = notebook.Name;
                context.Notebooks.AddOrUpdate(notebookToedit);
                context.SaveChanges();
                IsEditing = false;
                ReadNotebooks();
            }
        }
        public void UpdatedSelectedNote() {
            var noteToUpdate = context.Notes.Where(n => n.Id == selectedNote.Id).Single();
            if (noteToUpdate != null) {
                noteToUpdate.FileLocation = selectedNote.FileLocation;
                noteToUpdate.UpdatedTime = DateTime.Now;
                context.Notes.AddOrUpdate(noteToUpdate);
                context.SaveChanges();
            }
        }
    }
}
