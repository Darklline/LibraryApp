using LibraryApp.API.Services;
using LibraryApp.Data.Entities;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp.API.Controllers
{
    [ApiController]
    [Route("api/Authors/{authorId}/books")]
    public class BooksController : ControllerBase
    {
        private readonly ILibraryRepository libraryRepository;

        public BooksController(ILibraryRepository libraryRepository)
        {
            this.libraryRepository = libraryRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Book>> GetBooksForAuthor(int authorId)
        {
            if (!libraryRepository.AuthorExists(authorId)) return NotFound();

            var booksForAuthorFromRepo = libraryRepository.GetBooks(authorId);
            return Ok(booksForAuthorFromRepo);
        }

        [HttpGet("{bookId}", Name = "GetBookForAuthor")]
        public ActionResult<Book> GetBookForAuthor(int authorId, int bookId)
        {
            if (!libraryRepository.AuthorExists(authorId)) return NotFound();

            var bookForAuthorFromRepo = libraryRepository.GetBook(authorId, bookId);

            if (bookForAuthorFromRepo == null) return NotFound();

            return Ok(bookForAuthorFromRepo);
        }

        [HttpPost]
        public ActionResult<Book> CreateBookForAuthor(int authorId, Book book)
        {
            if (!libraryRepository.AuthorExists(authorId)) return NotFound();

            libraryRepository.AddBook(authorId, book);
            libraryRepository.Save();

            return CreatedAtRoute("GetBookForAuthor", new { bookId = book.Id, authorId = book.AuthorId }, book);
        }
    }
}
