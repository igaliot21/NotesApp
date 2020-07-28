using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp.Model
{
    public class NoteAppContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Notebook> Notebooks { get; set; }
        public DbSet<Note> Notes { get; set; }
    }
}
