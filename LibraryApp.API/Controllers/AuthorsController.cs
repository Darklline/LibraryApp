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
        public ActionResult<IEnumerable<Author>> GetAuthors()
        {
            return Ok(libraryRepository.GetAuthors());
        }
    }
}