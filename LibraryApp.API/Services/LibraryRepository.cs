﻿using LibraryApp.API.Helpers;
using LibraryApp.API.Models;
using LibraryApp.API.ResourceParameters;
using LibraryApp.Data.DbContexts;
using LibraryApp.Data.Entities;
using Microsoft.EntityFrameworkCore;
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
        private readonly IPropertyMappingService _propertyMappingService;

        public LibraryRepository(LibraryContext context, IPropertyMappingService propertyMappingService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _propertyMappingService = propertyMappingService ?? throw new ArgumentNullException(nameof(propertyMappingService));
        }
        public LibraryRepository(LibraryContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void AddAuthor(Author author)
        {
            if (author == null)
            {
                throw new ArgumentNullException(nameof(author));
            }

            _context.Authors.Add(author);
        }

        public bool AuthorExists(int authorId)
        {
            return _context.Authors.Any(a => a.Id == authorId);
        }

        public void DeleteAuthor(Author author)
        {
            if (author == null)
            {
                throw new ArgumentNullException(nameof(author));
            }
            _context.Authors.Remove(author);
        }

        public Author GetAuthor(int authorId)
        {
            return _context.Authors.FirstOrDefault(a => a.Id == authorId);
        }

        public IEnumerable<Author> GetAuthors()
        {
            return _context.Authors.ToList();
        }
        public PagedList<Author> GetAuthors(AuthorResourceParameters authorResourceParameters)
        {
            //if (string.IsNullOrWhiteSpace(authorResourceParameters.SearchQuery)) return _context.Authors.ToList();

            var collection = _context.Authors as IQueryable<Author>;

            if (!string.IsNullOrWhiteSpace(authorResourceParameters.SearchQuery))
            {
                authorResourceParameters.SearchQuery.Trim();
                collection = collection.Where(a => a.FirstName.Contains(authorResourceParameters.SearchQuery)
                || a.LastName.Contains(authorResourceParameters.SearchQuery));
            }
            if(!string.IsNullOrWhiteSpace(authorResourceParameters.OrderBy))
            {
                var authorPropertyMappingDictionary = _propertyMappingService.GetPropertyMapping<AuthorDto, Author>();


                collection = collection.ApplySort(authorResourceParameters.OrderBy, authorPropertyMappingDictionary);
            }

            return PagedList<Author>.Create(collection, 
                authorResourceParameters.PageNumber, 
                authorResourceParameters.PageSize);
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
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

        public IEnumerable<Book> GetBooks(int authorId)
        {
            return _context.Books.ToList().Where(b => b.AuthorId == authorId);
        }

        public Book GetBook(int authorId, int bookId)
        {
            return _context.Books.Where(b => b.AuthorId == authorId && b.Id == bookId).FirstOrDefault();
        }

        public void AddBook(int authorId, Book book)
        {
            if(book == null)
            {
                throw new ArgumentNullException(nameof(book));
            }
            book.AuthorId = authorId;
            _context.Books.Add(book);
        }

        public void UpdateBook(Book book)
        {
            _context.Entry(book).State = EntityState.Modified;
        }

        public void DeleteBook(Book book)
        {
            if(book == null)
            {
                throw new ArgumentNullException();
            }
            _context.Books.Remove(book);
        }
    }
}
