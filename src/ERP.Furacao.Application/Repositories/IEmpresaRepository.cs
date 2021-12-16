using ERP.Furacao.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Furacao.Application.Repositories
{
    public interface IEmpresaRepository : IEntityRepository<EmpresaModel>
    {

    }

    public interface IEmpresaDapperRepository : IDapperRepository<EmpresaModel>
    {
        Task<IEnumerable<EmpresaModel>> GetContasPorUsuarioAsync(int idUsuario);
    }
}
