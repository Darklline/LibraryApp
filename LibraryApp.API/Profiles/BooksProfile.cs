using AutoMapper;
using LibraryApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp.API.Profiles
{
    public class BooksProfile : Profile
    {
        public BooksProfile()
        {
            CreateMap<Book, Models.BookDto>();

            CreateMap<Models.BookForUpdateDto, Book>();
            CreateMap<Book, Models.BookForUpdateDto>();
        }
    }
}
