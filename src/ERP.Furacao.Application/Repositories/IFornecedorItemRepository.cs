using ERP.Furacao.Application.Pagination;
using ERP.Furacao.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Furacao.Application.Repositories
{
    public interface IFornecedorItemRepository
    {
        Task<bool> IncluirItensEmMassa(List<FornecedorItem> fornecedorItems);
        Task<PaginatedResult<FornecedorItem>> ListarItens(int page, int pageSize);
    }
}
