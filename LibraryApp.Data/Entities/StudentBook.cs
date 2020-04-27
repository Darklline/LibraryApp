using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryApp.Data.Entities
{
    public class StudentBook
    {
        public int StudentId { get; set; }
        public int BookId { get; set; }
        public Student Student { get; set; }
        public Book Book { get; set; }

    }
}
