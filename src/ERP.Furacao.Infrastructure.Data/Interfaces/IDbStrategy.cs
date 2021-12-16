using System.Data;

namespace ERP.Furacao.Infrastructure.Data.Interface
{
    public interface IDbStrategy
    {
        IDbConnection GetConnection(string connectionString);
    }
}
