using LibraryApp.Data.DbContexts;
using LibraryApp.Data.Entities;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp.API.Services
{
    public class LibraryRepository : ILibraryRepository, IDisposable
    {
        private readonly LibraryContext _context;
        public LibraryRepository(LibraryContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void AddAuthor(Author author)
        {
            throw new NotImplementedException();
        }

        public bool AuthorExists(int authorId)
        {
            if(authorId == 0)
            {
                throw new ArgumentNullException(nameof(authorId));
            }

            return _context.Authors.Any(a => a.Id == authorId);
        }

        public void DeleteAuthor(Author author)
        {
            throw new NotImplementedException();
        }

        public Author GetAuthor(int authorId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Author> GetAuthors()
        {
            return _context.Authors.ToList();
        }

        public bool save()
        {
            throw new NotImplementedException();
        }

        public void UpdateAuthor(Author author)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose resources when needed
            }
        }
    }
}
