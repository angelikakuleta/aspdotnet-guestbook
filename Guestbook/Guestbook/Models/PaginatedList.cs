using System;
using System.Collections.Generic;

namespace Guestbook.Models
{
    public class PaginatedList<T> : List<T>
    {
        public int PageNumber { get; private set; }
        public int TotalPages { get; private set; }
        public int TotalItems { get; private set; }

        public PaginatedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalItems = count;

            this.AddRange(items);
        }

        public bool HasPreviousPage => PageNumber > 1;

        public bool HasNextPage => PageNumber < TotalPages;
    }
}