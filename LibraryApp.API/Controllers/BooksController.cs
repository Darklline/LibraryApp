﻿using AutoMapper;
using LibraryApp.API.Models;
using LibraryApp.API.Services;
using LibraryApp.Data.Entities;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp.API.Controllers
{
    [ApiController]
    [Route("api/Authors/{authorId}/books")]
    //[ResponseCache(CacheProfileName = "240SecondsCacheProfile")]
    [HttpCacheExpiration(CacheLocation = CacheLocation.Public)]
    [HttpCacheValidation(MustRevalidate = true)]
    public class BooksController : ControllerBase
    {
        private readonly ILibraryRepository libraryRepository;
        private readonly IMapper mapper;

        public BooksController(ILibraryRepository libraryRepository, IMapper mapper)
        {
            this.libraryRepository = libraryRepository;
            this.mapper = mapper;
        }

        [HttpGet(Name = "GetBooksForAuthor")]
        public ActionResult<IEnumerable<Book>> GetBooksForAuthor(int authorId)
        {
            if (!libraryRepository.AuthorExists(authorId)) return NotFound();

            var booksForAuthorFromRepo = libraryRepository.GetBooks(authorId);
            return Ok(mapper.Map<IEnumerable<BookDto>>(booksForAuthorFromRepo));
        }

        [HttpGet("{bookId}", Name = "GetBookForAuthor")]
        public ActionResult<Book> GetBookForAuthor(int authorId, int bookId)
        {
            if (!libraryRepository.AuthorExists(authorId)) return NotFound();

            var bookForAuthorFromRepo = libraryRepository.GetBook(authorId, bookId);

            if (bookForAuthorFromRepo == null) return NotFound();

            return Ok(mapper.Map<BookDto>(bookForAuthorFromRepo));
        }

        [HttpPost(Name = "CreateBookForAuthor")]
        public ActionResult<Book> CreateBookForAuthor(int authorId, Book book)
        {
            if (!libraryRepository.AuthorExists(authorId)) return NotFound();

            libraryRepository.AddBook(authorId, book);
            libraryRepository.Save();

            return CreatedAtRoute("GetBookForAuthor", new { bookId = book.Id, authorId = book.AuthorId }, book);
        }

        [HttpPut("{bookId}")]
        public ActionResult UpdateBookForAuthor(int authorId, int bookId, BookForUpdateDto book)
        {
            if (!libraryRepository.AuthorExists(authorId)) return NotFound();

            var bookForAuthorFromRepo = libraryRepository.GetBook(authorId, bookId);

            if (bookForAuthorFromRepo == null) return NotFound();

            mapper.Map(book, bookForAuthorFromRepo);

            libraryRepository.UpdateBook(bookForAuthorFromRepo);

            libraryRepository.Save();

            return NoContent();
        }
        [HttpPatch("{bookId}")]
        public ActionResult PatchingBookForAuthor(int authorId, int bookId, JsonPatchDocument<BookForUpdateDto> patchDocument)
        {
            if (!libraryRepository.AuthorExists(authorId)) return NotFound();

            var bookForAuthorFromRepo = libraryRepository.GetBook(authorId, bookId);

            if (bookForAuthorFromRepo == null) return NotFound();

            var bookToPatch = mapper.Map<BookForUpdateDto>(bookForAuthorFromRepo);
            // validate
            patchDocument.ApplyTo(bookToPatch);

            //if (TryValidateModel(bookToPatch)) return ValidationProblem(ModelState);
            

            mapper.Map(bookToPatch, bookForAuthorFromRepo);

            libraryRepository.UpdateBook(bookForAuthorFromRepo);
            libraryRepository.Save();

            return NoContent();
        }
        //public override ActionResult ValidationProblem([ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
        //{
        //   var options = HttpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>();
        //  return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);  
        // }
        [HttpDelete("{bookId}")]
        public ActionResult DeleteBook(int authorId, int bookId)
        {
            if (!libraryRepository.AuthorExists(authorId)) return NotFound();

            var bookForAuthorFromRepo = libraryRepository.GetBook(authorId, bookId);

            if (bookForAuthorFromRepo == null) return NotFound();

            libraryRepository.DeleteBook(bookForAuthorFromRepo);
            libraryRepository.Save();
            return NoContent();
        }
        [HttpOptions]
        public IActionResult GetBooksOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST,DELETE,PUT,PATCH");
            return Ok();
        }
    }
}
