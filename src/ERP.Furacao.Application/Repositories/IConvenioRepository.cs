using ERP.Furacao.Application.DTOs.Convenio;
using ERP.Furacao.Domain.Models;
using System.Threading.Tasks;

namespace ERP.Furacao.Application.Repositories
{
    public interface IConvenioRepository : IEntityRepository<ConvenioModel>
    {

    }

    public interface IConvenioDapperRepository : IDapperRepository<ConvenioModel>
    {
        Task<DadoDoConvenioResponse> GetConvenioPorProcedureAsync(string procedureName, DadoDoConvenioRequest request);
    }
}
