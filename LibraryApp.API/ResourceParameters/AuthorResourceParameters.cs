using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp.API.ResourceParameters
{
    public class AuthorResourceParameters
    {
        const int maxPageSize = 10;
        public string SearchQuery { get; set; }
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 5;
        public int PageSize 
        {
            get => _pageSize; 
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value; 
        }
        public string OrderBy { get; set; } = "FirstName";

    }
}
