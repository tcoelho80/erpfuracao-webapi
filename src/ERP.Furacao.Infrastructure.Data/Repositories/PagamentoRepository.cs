using Dapper;
using Infrastructure.Data.Repositories;
using ERP.Furacao.Application.DTOs.Pagamento;
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
    public class PagamentoRepository : EntityBaseRepository<PagamentoModel>, IPagamentoRepository
    {
        private readonly ApplicationContext _context;
        public PagamentoRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }
    }

    public class PagamentoDapperRepository : DapperBaseRepository<PagamentoModel>, IPagamentoDapperRepository
    {
        public PagamentoDapperRepository(DapperDbSettings dbSettings) : base(dbSettings)
        {

        }

        public async Task<PagamentoResponse> IncluirPagamentoPorProcedureAsync(string procedureName, PagamentoBaseRequest request)
        {
            OracleParametersHelper.Entity = request;
            var parameters = OracleParametersHelper.Parameters;

            await DbConnection.QueryAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);

            return new PagamentoResponse
            {
                PONCodigoRetorno = parameters.Get<int>("PONCODIGORETORNO"),
                POSMsgRetorno = parameters.Get<string>("POSMSGRETORNO"),
                IdPagamento = parameters.Get<int>("PONIDPAGAMENTO")
            };
        }

        public async Task<PagamentoSituacaoResponse> GetComprovantePorProcedureAsync(string procedureName, PagamentoSituacaoRequest request)
        {
            OracleParametersHelper.Entity = request;
            var parameters = OracleParametersHelper.Parameters;

            var dados = await DbConnection.QueryAsync<ComprovanteResponse>(procedureName, parameters, commandType: CommandType.StoredProcedure);

            return new PagamentoSituacaoResponse
            {
                PONCodigoRetorno = parameters.Get<int>("PONCODIGORETORNO"),
                POSMsgRetorno = parameters.Get<string>("POSMSGRETORNO"),
                POCurDados = dados.To<List<ComprovanteResponse>>()
            };
        }
    }
}
