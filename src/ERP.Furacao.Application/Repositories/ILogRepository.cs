using ERP.Furacao.Application.DTOs.Log;
using ERP.Furacao.Domain.Models;

namespace ERP.Furacao.Application.Repositories
{
    public interface ILogRepository : IEntityRepository<LogModel>
    {

    }

    public interface ILogDapperRepository : IDapperRepository<LogModel>
    {
        LogResponse GetLogs(LogModel filter);
    }
}
