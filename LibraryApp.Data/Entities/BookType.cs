using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryApp.Data.Entities
{
    public class BookType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int BookId { get; set; }
    }
}
