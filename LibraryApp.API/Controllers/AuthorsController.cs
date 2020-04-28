using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Data.DbContexts;
using LibraryApp.Data.Entities;
using LibraryApp.API.Services;

namespace LibraryApp.API.Controllers
{
    [ApiController]
    [Route("api/authors")]
    public class AuthorsController : ControllerBase
    {
        private readonly ILibraryRepository libraryRepository;

        public AuthorsController(ILibraryRepository libraryRepository)
        {
            this.libraryRepository = libraryRepository;
        }

        [HttpGet()]
        [HttpHead]
        public ActionResult<IEnumerable<Author>> GetAuthors()
        {
            var authorsFromRepo = libraryRepository.GetAuthors();

            return Ok(authorsFromRepo);
        }

        [HttpGet("{authorId}", Name = "GetAuthor")]
        public IActionResult GetAuthor(int authorId)
        {
            var authorFromRepo = libraryRepository.GetAuthor(authorId);

            if (authorFromRepo == null) return NotFound();

            return Ok(authorFromRepo);
        }

        [HttpPost]
        public ActionResult<Author> CreateAuthor(Author author)
        {
            libraryRepository.AddAuthor(author);
            libraryRepository.Save();

            return CreatedAtRoute("GetAuthor", new { authorId = author.Id }, author);
        }

        [HttpOptions]
        public IActionResult GetAuthorsOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST");
            return Ok();
        }
    }
}