using AutoMapper;
using ERP.Furacao.Application.DTOs.Pagamento;
using ERP.Furacao.Application.Repositories;
using ERP.Furacao.Infrastructure.Crosscutting.Interfaces.Logs;
using ERP.Furacao.Infrastructure.Data.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ERP.Furacao.WebApi.Controllers.v1.Pagamentos
{
    /// <summary>
    /// Conta Controller
    /// </summary>
    [Authorize]
    [ApiVersion("1.0")]
    [ControllerName("Pagamentos")]
    public class PagamentosController : BaseController
    {
        private readonly IPagamentoDapperRepository _pagamentoDapperRepository;

        public PagamentosController(IPagamentoDapperRepository pagamentoDapperRepository,
                                    ILogService logService,
                                    IMapper mapper)
            : base(mapper, logService)
        {
            _pagamentoDapperRepository = pagamentoDapperRepository;
            _pagamentoDapperRepository.SetProvider(ProviderTypeEnum.Oracle);
        }


        [HttpPost("incluir-pagamento-entre-contas-internas")]
        public async Task<IActionResult> IncluirPagamentoEntreContasInternas([FromBody] PagamentoContaRequest request)
        {
            var response = await DoInLog(async () =>
               await _pagamentoDapperRepository.IncluirPagamentoPorProcedureAsync("SCVAPITOKYOINSEREPAGAMENTO", request),
               request,
               "incluir-pagamento-entre-contas-internas");

            return Ok(response);
        }


        [HttpPost("incluir-pagamento-ted")]
        public async Task<IActionResult> IncluirPagamentoDeTED([FromBody] PagamentoTEDRequest request)
        {
            var response = await DoInLog(async () =>
               await _pagamentoDapperRepository.IncluirPagamentoPorProcedureAsync("SCVAPITOKYOINSEREPAGAMENTO", request),
               request,
               "incluir-pagamento-ted");

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("obter-comprovante-de-pagamento-boleto")]
        public async Task<IActionResult> ObterSituacaoDoPagamentoBoleto([FromQuery] PagamentoSituacaoRequest request)
        {
            var response = await DoInLog(async () =>
                await _pagamentoDapperRepository.GetComprovantePorProcedureAsync("SCVAPITOKYOCONFRANCANALITICA", request),
                request,
                "obter-comprovante-de-pagamento-boleto");

            return Ok(JsonConvert.SerializeObject(response, Formatting.Indented));
        }

        [AllowAnonymous]
        [HttpGet("obter-comprovante-de-pagamento-ted")]
        public async Task<IActionResult> ObterSituacaoDoPagamentoTED([FromQuery] PagamentoSituacaoRequest request)
        {
            var response = await DoInLog(async () =>
                await _pagamentoDapperRepository.GetComprovantePorProcedureAsync("SCVAPITOKYOCONFRANCANALITICA", request),
                request,
                "obter-comprovante-de-pagamento-ted");

            return Ok(JsonConvert.SerializeObject(response, Formatting.Indented));
        }
    }
}
