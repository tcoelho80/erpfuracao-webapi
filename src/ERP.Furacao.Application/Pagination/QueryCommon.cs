namespace ERP.Furacao.Application.Pagination
{
    public class QueryCommon
    {
        public static string TotalItemsQuery(string query)
        {
            return $"SELECT COUNT(*) FROM ({query}) t";
        }

        public static string PagedQuery(string query, int page, int pageSize)
        {
            return $"{query} OFFSET {(page - 1) * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY";
        }
    }
}
