using ERP.Furacao.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Furacao.Application.Repositories
{
    public interface IDapperRepository<T> where T : class
    {
        void SetProvider(object providerType);
        void ExecuteProcedure(string name);
        void ExecuteProcedure(string name, object parameters);
        T ExecuteProcedureFirstOrDefault(string name, object parameters);
        Task<T> ExecuteProcedureFirstOrDefaultAsync(string name, object parameters);
        Task<IEnumerable<T>> ExecuteProcedureQueryAsync(ProcedureRequest procedure);
        IEnumerable<T> ExecuteProcedureQuery(string name);
        IEnumerable<T> ExecuteProcedureQuery(ProcedureRequest procedure);
        IEnumerable<T> GetAll(string table = "");
        IEnumerable<T> GetExecuteQuery(string query);
        Task<IEnumerable<T>> GetExecuteQueryAsync(string query);
        IEnumerable<T> ExecuteSelect(string command);
    }
}
