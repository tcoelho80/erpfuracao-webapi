using Dapper;
using ERP.Furacao.Application.DTOs;
using ERP.Furacao.Application.Repositories;
using ERP.Furacao.Domain.Extensions;
using ERP.Furacao.Infrastructure.Data.Enums;
using ERP.Furacao.Infrastructure.Data.Helpers;
using ERP.Furacao.Infrastructure.Data.Settings;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class DapperBaseRepository<T> : IDapperRepository<T> where T : class
    {
        protected IDbConnection DbConnection { get; private set; }
        protected DapperDbSettings DbSettings { get; private set; }
        public static int TimeOut { get { return 2000; } }

        public DapperBaseRepository(DapperDbSettings dbSettings)
        {
            DbSettings = dbSettings;
            DbConnection = dbSettings
                            .SetStrategy(dbSettings.ProviderType)
                            .GetDbContext(dbSettings.GetConnectionString());

            ConnectionHelper<T>.DbConnection = DbConnection;
        }

        public void SetProvider(object providerType)
        {
            var dapperSettings = new DapperDbSettings()
            {
                ProviderType = providerType.To<ProviderTypeEnum>()
            };

            DbConnection = dapperSettings
                .SetStrategy(dapperSettings.ProviderType)
                .GetDbContext(dapperSettings.GetConnectionString(DbSettings));
        }

        public async Task<T> ExecuteProcedureFirstOrDefaultAsync(string name, object parameters)
        {
            return await ConnectionHelper<T>.ExecuteFirst(async () =>
               await DbConnection.QueryFirstOrDefaultAsync<T>(name, parameters, commandType: CommandType.StoredProcedure, commandTimeout: TimeOut)
            );
        }

        public async Task<IEnumerable<T>> ExecuteProcedureQueryAsync(ProcedureRequest procedure)
        {
            return await ConnectionHelper<T>.ExecuteList(async () =>
              await DbConnection.QueryAsync<T>(procedure.Name, procedure.ParametersProcedure, commandType: CommandType.StoredProcedure, commandTimeout: TimeOut).ConfigureAwait(true)
            );
        }

        public void ExecuteProcedure(string name)
        {
            ConnectionHelper<T>.Execute(() =>
             DbConnection.Execute(name, commandType: CommandType.StoredProcedure, commandTimeout: TimeOut)
           );
        }

        public void ExecuteProcedure(string name, object parameters)
        {
            ConnectionHelper<T>.Execute(() =>
             DbConnection.Execute(sql: name, param: parameters, commandType: CommandType.StoredProcedure, commandTimeout: TimeOut)
           );
        }

        public T ExecuteProcedureFirstOrDefault(string name, object parameters)
        {
            return ConnectionHelper<T>.ExecuteFirst(() =>
             DbConnection.QueryFirstOrDefault<T>(name, parameters, commandType: CommandType.StoredProcedure, commandTimeout: TimeOut)
           );
        }

        public IEnumerable<T> ExecuteProcedureQuery(string name)
        {
            return ConnectionHelper<T>.ExecuteList(() =>
              DbConnection.Query<T>(name, commandType: CommandType.StoredProcedure, commandTimeout: TimeOut)
           );
        }

        public IEnumerable<T> ExecuteProcedureQuery(ProcedureRequest procedure)
        {
            return ConnectionHelper<T>.ExecuteList(() =>
                DbConnection.Query<T>(procedure.Name, procedure.ParametersProcedure, commandType: CommandType.StoredProcedure, commandTimeout: TimeOut)
            );
        }

        public IEnumerable<T> GetAll(string table = "")
        {
            var query = $"SELECT * FROM {(string.IsNullOrEmpty(table) ? typeof(T).Name : table)}";
            return ConnectionHelper<T>.ExecuteList(() =>
              DbConnection.Query<T>(query, commandTimeout: TimeOut)
             );
        }


        public IEnumerable<T> GetExecuteQuery(string query)
        {
            return ConnectionHelper<T>.ExecuteList(() =>
             DbConnection.Query<T>(query, commandTimeout: TimeOut)
            );
        }
        public IEnumerable<T> GetExecuteQuery(string query, object parameters)
        {
            return ConnectionHelper<T>.ExecuteList(() =>
             DbConnection.Query<T>(query, parameters, commandTimeout: TimeOut)
            );
        }


        public async Task<IEnumerable<T>> GetExecuteQueryAsync(string query)
        {
            return await ConnectionHelper<T>.ExecuteList(async () =>
             await DbConnection.QueryAsync<T>(query, commandTimeout: TimeOut)
            );
        }

        public async Task<IEnumerable<T>> GetExecuteQueryAsync(string query, object parameters)
        {
            return await ConnectionHelper<T>.ExecuteList(async () =>
             await DbConnection.QueryAsync<T>(query, parameters, commandTimeout: TimeOut)
            );
        }

        public IEnumerable<T> ExecuteSelect(string command)
        {
            return ConnectionHelper<T>.ExecuteList(() =>
             DbConnection.Query<T>(command, commandType: CommandType.Text, commandTimeout: TimeOut, buffered: false)
            );
        }

        public IEnumerable<T> ExecuteSelect(string command, object parameters)
        {
            return ConnectionHelper<T>.ExecuteList(() =>
             DbConnection.Query<T>(command, parameters, commandType: CommandType.Text, commandTimeout: TimeOut, buffered: false)
            );
        }
    }
}
