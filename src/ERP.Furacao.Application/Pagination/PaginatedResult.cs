using System.Collections.Generic;

namespace ERP.Furacao.Application.Pagination
{
    public class PaginatedResult<T> where T : class
    {
        public List<T> Data { get; set; }
        public int TotalItems { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
