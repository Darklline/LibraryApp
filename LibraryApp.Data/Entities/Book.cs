using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryApp.Data.Entities
{
    public class Book
    {
        public Book()
        {
            StudentBooks = new List<StudentBook>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int PageCount { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        public BookType BookType { get; set; }
        public List<StudentBook> StudentBooks { get; set; }
    }
}
