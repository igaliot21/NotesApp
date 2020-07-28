using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NotesApp.Model
{
    public class Note : INotifyPropertyChanged
    {
        private int id;
        private Notebook notebook;
        private int notebookid;
        private string title;
        private DateTime createdtime;
        private DateTime updatedtime;
        private string filelocation;

        public int Id
        {
            get { return id; }
            set { 
                id = value;
                OnPropertyChange("Id");
            }
        }
        public Notebook Notebook
        {
            get { return notebook; }
            set { 
                notebook = value;
                OnPropertyChange("Notebook");
            }
        }
        public int NotebookId
        {
            get { return notebookid; }
            set { 
                notebookid = value;
                OnPropertyChange("NotebookId");
            }
        }
        [Required]
        [MaxLength(255)]
        public string Title
        {
            get { return title; }
            set { 
                title = value;
                OnPropertyChange("Title");
            }
        }
        public DateTime CreatedTime
        {
            get { return createdtime; }
            set { 
                createdtime = value;
                OnPropertyChange("CreatedTime");
            }
        }

        public DateTime UpdatedTime
        {
            get { return updatedtime; }
            set { 
                updatedtime = value;
                OnPropertyChange("UpdatedTime");
            }
        }
        public string FileLocation
        {
            get { return filelocation; }
            set { 
                filelocation = value;
                OnPropertyChange("FileLocation");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public Note(){}
        public Note(string Title, string FileLocation, Notebook Notebook){
            this.title = Title;
            this.filelocation = FileLocation;
            this.notebook = Notebook;
            this.notebookid = Notebook.Id;
            this.createdtime = DateTime.Now;
            this.updatedtime = DateTime.Now;
        }
        private void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
