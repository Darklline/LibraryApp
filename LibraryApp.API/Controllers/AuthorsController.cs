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
using LibraryApp.API.Models;
using AutoMapper;
using LibraryApp.API.ResourceParameters;
using System.Security.AccessControl;
using LibraryApp.API.Helpers;
using System.Text.Json;
using Microsoft.Net.Http.Headers;

namespace LibraryApp.API.Controllers
{
    [ApiController]
    [Route("api/authors")]
    public class AuthorsController : ControllerBase
    {
        private readonly ILibraryRepository libraryRepository;
        private readonly IMapper mapper;
        private readonly IPropertyMappingService propertyMappingService;
        private readonly IPropertyCheckerService propertyCheckerService;

        public AuthorsController(ILibraryRepository libraryRepository, IMapper mapper, IPropertyMappingService propertyMappingService, IPropertyCheckerService propertyCheckerService)
        {
            this.libraryRepository = libraryRepository ?? throw new ArgumentNullException(nameof(libraryRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.propertyMappingService = propertyMappingService ?? throw new ArgumentNullException(nameof(propertyMappingService));
            this.propertyCheckerService = propertyCheckerService ?? throw new ArgumentNullException(nameof(propertyCheckerService));
        }

        [HttpGet(Name = "GetAuthors")]
        [HttpHead]
        public IActionResult GetAuthors([FromQuery] AuthorResourceParameters authorResourceParameters)
        {
            if(!propertyMappingService.ValidMappingExistsFor<AuthorDto, Author>(authorResourceParameters.OrderBy))
            {
                return BadRequest();
            }

            if(!propertyCheckerService.TypeHasProperties<AuthorDto>(authorResourceParameters.Fields))
            {
                return BadRequest();
            }

            var authorsFromRepo = libraryRepository.GetAuthors(authorResourceParameters);

            var paginationMetadata = new
            {
                totalCount = authorsFromRepo.TotalCount,
                pageSize = authorsFromRepo.PageSize,
                currentPage = authorsFromRepo.CurrentPage,
                totalPages = authorsFromRepo.TotalPages
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            var links = CreateLinksForAuthors(authorResourceParameters, authorsFromRepo.HasNext, authorsFromRepo.HasPrevious);

            var shapedAuthors = mapper.Map<IEnumerable<AuthorDto>>(authorsFromRepo).ShapeData(authorResourceParameters.Fields);

            var shapedAuthorsWithLinks = shapedAuthors.Select(author =>
            {
                var authorAsDictionary = author as IDictionary<string, object>;
                var authorLinks = CreateLinksForAuthor((int)authorAsDictionary["Id"], null);
                authorAsDictionary.Add("links", authorLinks);
                return authorAsDictionary;
            });

            var linkedCollectionResource = new
            {
                value = shapedAuthorsWithLinks,
                links
            };

            return Ok(linkedCollectionResource);
        }

        [HttpGet("{authorId}", Name = "GetAuthor")]
        public IActionResult GetAuthor(int authorId, string fields, [FromHeader(Name = "Accept")] string mediaType)
        {
            if(!MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue parsedMediaType))
            {
                return BadRequest();
            }

            if (!propertyCheckerService.TypeHasProperties<AuthorDto>(fields))
            {
                return BadRequest();
            }

            var authorFromRepo = libraryRepository.GetAuthor(authorId);

            if (authorFromRepo == null)
            {
                return NotFound();
            }

            if (parsedMediaType.MediaType == "application/vnd.wojtek.hateoas+json")
            {

                var links = CreateLinksForAuthor(authorId, fields);

                var linkedResourceToReturn = mapper.Map<AuthorDto>(authorFromRepo).ShapeData(fields) as IDictionary<string, object>;

                linkedResourceToReturn.Add("links", links);

                return Ok(linkedResourceToReturn);
            }

            return Ok(mapper.Map<AuthorDto>(authorFromRepo).ShapeData(fields));
        }

        [HttpPost(Name ="CreateAuthor")]
        public ActionResult<Author> CreateAuthor(Author author)
        {
            libraryRepository.AddAuthor(author);
            libraryRepository.Save();

            var links = CreateLinksForAuthor(author.Id, null);

            var linkedResourceToReturn = author.ShapeData(null) as IDictionary<string, object>;
            linkedResourceToReturn.Add("links", links);

            return CreatedAtRoute("GetAuthor", new { authorId = linkedResourceToReturn["Id"] }, linkedResourceToReturn);
        }
        [HttpDelete("{authorId}", Name = "DeleteAuthor")]
        public ActionResult DeleteAuthor(int authorId)
        {
            var authorFromRepo = libraryRepository.GetAuthor(authorId);

            if (authorFromRepo == null) return NotFound();

            libraryRepository.DeleteAuthor(authorFromRepo);
            libraryRepository.Save();
            return NoContent();
        }

        [HttpOptions]
        public IActionResult GetAuthorsOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST,DELETE");
            return Ok();
        }
        private string CreateAuthorsResourceUri(AuthorResourceParameters authorResourceParameters, ResourceUriType type)
        {
            switch(type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link("GetAuthors",
                        new
                        {
                            fields = authorResourceParameters.Fields,
                            orderBy = authorResourceParameters.OrderBy,
                            pageNumber = authorResourceParameters.PageNumber - 1,
                            pageSize = authorResourceParameters.PageSize,
                            searchQuery = authorResourceParameters.SearchQuery
                        });
                case ResourceUriType.NextPage:
                    return Url.Link("GetAuthors",
                        new
                        {
                            fields = authorResourceParameters.Fields,
                            orderBy = authorResourceParameters.OrderBy,
                            pageNumber = authorResourceParameters.PageNumber + 1,
                            pageSize = authorResourceParameters.PageSize,
                            searchQuery = authorResourceParameters.SearchQuery
                        });
                case ResourceUriType.Current:
                default:
                    return Url.Link("GetAuthors",
                        new
                        {
                            fields = authorResourceParameters.Fields,
                            orderBy = authorResourceParameters.OrderBy,
                            pageNumber = authorResourceParameters.PageNumber,
                            pageSize = authorResourceParameters.PageSize,
                            searchQuery = authorResourceParameters.SearchQuery
                        });
            }
        }
        private IEnumerable<LinkDto> CreateLinksForAuthor(int authorId, string fields)
        {
            var links = new List<LinkDto>();

            if(string.IsNullOrWhiteSpace(fields))
            {
                links.Add(
                    new LinkDto(Url.Link("GetAuthor", new { authorId }), "self", "GET"));
            }
            else
            {
                links.Add(
                    new LinkDto(Url.Link("GetAuthor", new { authorId, fields }), "self", "GET"));
            }

            links.Add(
                new LinkDto(Url.Link("DeleteAuthor", new { authorId }), "delete_author", "DELETE"));

            links.Add(
                new LinkDto(Url.Link("CreateBookForAuthor", new { authorId }), "create_book_for_author", "POST"));

            links.Add(
                new LinkDto(Url.Link("GetBooksForAuthor", new { authorId }), "books", "GET"));
            

            return links;
        }
        private IEnumerable<LinkDto> CreateLinksForAuthors(AuthorResourceParameters authorResourceParameters, bool hasNext, bool hasPrevious)
        {
            var links = new List<LinkDto>();

            links.Add(new LinkDto(CreateAuthorsResourceUri(authorResourceParameters, ResourceUriType.Current), "self", "GET"));

            if(hasNext)
            {
                links.Add(new LinkDto(CreateAuthorsResourceUri(authorResourceParameters, ResourceUriType.NextPage), "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(new LinkDto(CreateAuthorsResourceUri(authorResourceParameters, ResourceUriType.PreviousPage), "previousPage", "GET"));
            }

            return links;
        }
    }
}