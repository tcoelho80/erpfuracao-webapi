using ERP.Furacao.Infrastructure.Data.Interface;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace ERP.Furacao.Infrastructure.Data.Strategies
{
    public class OracleStrategy : IDbStrategy
    {
        public IDbConnection GetConnection(string connectionString)
        {
            return new OracleConnection(connectionString);
        }
    }
}
