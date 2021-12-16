using Dapper;
using Infrastructure.Data.Repositories;
using ERP.Furacao.Application.DTOs.Convenio;
using ERP.Furacao.Application.Repositories;
using ERP.Furacao.Domain.Extensions;
using ERP.Furacao.Domain.Models;
using ERP.Furacao.Infrastructure.Data.Contexts;
using ERP.Furacao.Infrastructure.Data.Helpers;
using ERP.Furacao.Infrastructure.Data.Settings;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ERP.Furacao.Infrastructure.Data.Repositories
{
    public class ConvenioRepository : EntityBaseRepository<ConvenioModel>, IConvenioRepository
    {
        public ConvenioRepository(ApplicationContext context) : base(context)
        {
        }
    }

    public class ConvenioDapperRepository : DapperBaseRepository<ConvenioModel>, IConvenioDapperRepository
    {
        public ConvenioDapperRepository(DapperDbSettings dbSettings) : base(dbSettings)
        {

        }

        public async Task<DadoDoConvenioResponse> GetConvenioPorProcedureAsync(string procedureName, DadoDoConvenioRequest request)
        {
            OracleParametersHelper.Entity = request;
            var parameters = OracleParametersHelper.Parameters;

            var dados = await DbConnection.QueryAsync<int>(procedureName, parameters, commandType: CommandType.StoredProcedure);

            return new DadoDoConvenioResponse
            {
                CodigoRetorno = parameters.Get<int>("PONCODIGORETORNO"),
                MensagemRetorno = parameters.Get<string>("POSMSGRETORNO"),
                IdsConvenios = dados.To<List<int>>()
            };
        }
    }
}
