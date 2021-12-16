using ERP.Furacao.Application.DTOs.Pagamento;
using ERP.Furacao.Domain.Models;
using System.Threading.Tasks;

namespace ERP.Furacao.Application.Repositories
{
    public interface IPagamentoRepository : IEntityRepository<PagamentoModel>
    {

    }

    public interface IPagamentoDapperRepository : IDapperRepository<PagamentoModel>
    {
        Task<PagamentoResponse> IncluirPagamentoPorProcedureAsync(string procedureName, PagamentoBaseRequest request);

        Task<PagamentoSituacaoResponse> GetComprovantePorProcedureAsync(string procedureName, PagamentoSituacaoRequest request);
    }
}
