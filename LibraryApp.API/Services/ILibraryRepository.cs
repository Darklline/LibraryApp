using LibraryApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp.API.Services
{
    public interface ILibraryRepository
    {
        void AddBook(int authorId, Book book);
        Book GetBook(int authorId, int bookId);
        IEnumerable<Book> GetBooks(int authorId);
        IEnumerable<Author> GetAuthors();
        Author GetAuthor(int authorId);
        void AddAuthor(Author author);
        void DeleteAuthor(Author author);
        void UpdateAuthor(Author author);
        bool AuthorExists(int authorId);
        bool Save();
        
    }
}
