using Infrastructure.Data.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using ERP.Furacao.Application.Repositories;
using ERP.Furacao.Domain.Models;
using ERP.Furacao.Infrastructure.Data.Contexts;
using ERP.Furacao.Infrastructure.Data.Settings;

namespace ERP.Furacao.Infrastructure.Data.Repositories
{
    public class EmpresaRepository : EntityBaseRepository<EmpresaModel>, IEmpresaRepository
    {
        public EmpresaRepository(ApplicationContext context) : base(context)
        {
        }
    }

    public class EmpresaDapperRepository : DapperBaseRepository<EmpresaModel>, IEmpresaDapperRepository
    {
        public EmpresaDapperRepository(DapperDbSettings dbSettings) : base(dbSettings)
        {

        }

        public async Task<IEnumerable<EmpresaModel>> GetContasPorUsuarioAsync(int idUsuario)
        {
            return await base.GetExecuteQueryAsync($"SELECT * FROM TBIBEMPRESA WHERE IDUSUARIO = {idUsuario}");
        }
    }
}
