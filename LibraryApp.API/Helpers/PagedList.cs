using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp.API.Helpers
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public bool HasNext => (CurrentPage < TotalPages);
        public bool HasPrevious => (CurrentPage > 1);
        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }
        public static PagedList<T> Create(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            if (pageNumber == 0)
            {
                 var items = source.Take(pageSize).ToList();
                 return new PagedList<T>(items, count, pageNumber, pageSize);
            }
            else
            {
                 var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                 return new PagedList<T>(items, count, pageNumber, pageSize);
            }
            
        }
    }
}
