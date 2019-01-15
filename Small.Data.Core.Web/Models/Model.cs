using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Small.Data.Core.Web.Models
{
    public class NotesContext : DbContext
    {
        public NotesContext(DbContextOptions<NotesContext> options)
            : base(options)
        { }

        public DbSet<Note> Notes { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlite("Data Source=C:\\Data\\AppData\\SmallData.db");
        //}
    }

    public class Note
    {
        public string Name { get; set; }

        public string Detail { get; set; }

        public string Category { get; set; }

        public int NoteID { get; set; }
    }

}
