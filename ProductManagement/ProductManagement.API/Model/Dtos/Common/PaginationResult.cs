﻿namespace ProductManagement.API.Model.Dtos.Common
{
    public class PaginationResult<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling((double) TotalCount / PageSize);
    }
}
