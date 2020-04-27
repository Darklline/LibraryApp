using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryApp.Data.Entities
{
    public class Author
    {
        public Author()
        {

        }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Book> Books { get; set; }

    }
}
