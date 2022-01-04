using System.Data;
using ERP.Furacao.Domain.Extensions;
using ERP.Furacao.Infrastructure.Data.Enums;
using ERP.Furacao.Infrastructure.Data.Interface;
using ERP.Furacao.Infrastructure.Data.Strategies;

namespace ERP.Furacao.Infrastructure.Data.Settings
{
    public class DapperDbSettings
    {
        private IDbStrategy _dbStrategy;
        public ProviderTypeEnum ProviderType { get; set; } = ProviderTypeEnum.Oracle;
        public string SqlServerConnectionString { get; set; }
        public string OracleConnectionString { get; set; }

        public DapperDbSettings SetStrategy(ProviderTypeEnum providerType)
        {
            _dbStrategy = providerType switch
            {
                ProviderTypeEnum.SqlServer => _dbStrategy = new SqlServerStrategy(),
                ProviderTypeEnum.Oracle => _dbStrategy = new OracleStrategy(),
                _ => null
            };

            return this;
        }

        public string GetConnectionString(DapperDbSettings dbSettingsOld = null) => this.ProviderType switch
        {
            ProviderTypeEnum.SqlServer => dbSettingsOld.IsNotNull() ? dbSettingsOld.SqlServerConnectionString : this.SqlServerConnectionString,
            ProviderTypeEnum.Oracle => dbSettingsOld.IsNotNull() ? dbSettingsOld.OracleConnectionString : this.OracleConnectionString,
            _ => null
        };


        public IDbConnection GetDbContext(string connectionString)
        {
            return _dbStrategy.GetConnection(connectionString);
        }
    }
}
