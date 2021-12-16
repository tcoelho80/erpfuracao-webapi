using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ERP.Furacao.Application.DTOs;
using ERP.Furacao.Application.DTOs.Conta;
using ERP.Furacao.Application.DTOs.Convenio;
using ERP.Furacao.Application.Repositories;
using ERP.Furacao.Domain.Extensions;
using ERP.Furacao.Domain.Models;
using ERP.Furacao.Infrastructure.Crosscutting.Interfaces.Logs;
using ERP.Furacao.Infrastructure.Data.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Furacao.WebApi.Controllers.v1.Contas
{
    /// <summary>
    /// Conta Controller
    /// </summary>
    [Authorize]
    [ApiVersion("1.0")]
    [ControllerName("Contas")]
    public class ContasController : BaseController
    {
        private readonly IEmpresaDapperRepository _empresaDapperRepository;
        private readonly IConvenioDapperRepository _convenioDapperRepository;

        public ContasController(IEmpresaDapperRepository empresaDapperRepository,
                                IConvenioDapperRepository convenioDapperRepository,
                                ILogService logService,
                                IMapper mapper)
            : base(mapper, logService)
        {
            _empresaDapperRepository = empresaDapperRepository;
            _convenioDapperRepository = convenioDapperRepository;
        }

        [HttpGet("obter-dados-das-empresas")]
        public async Task<IActionResult> ObterDadosDasEmpresas([FromQuery] DadoDaEmpresaRequest request)
        {
            var response = await DoInLog(async () =>
                await _empresaDapperRepository.GetContasPorUsuarioAsync(request.IdUsuario),
                request,
                "obter-dados-das-empresas"
            );

            return Ok(_mapper.Map<List<EmpresaModel>, List<DadoDaEmpresaResponse>>(response.To<List<EmpresaModel>>()));
        }

        [HttpGet("obter-dados-das-contas")]
        public async Task<IActionResult> ObterDadosDasContas([FromQuery] DadoDaContaRequest request)
        {
            var response = await DoInLog(async () =>
                await _empresaDapperRepository.GetContasPorUsuarioAsync(request.IdUsuario),
                request,
                "obter-dados-das-contas"
            );

            return Ok(_mapper.Map<List<EmpresaModel>, List<DadoDaContaResponse>>(response.To<List<EmpresaModel>>()));
        }

        [HttpGet("obter-empresas-contas")]
        public async Task<IActionResult> ObterDadosEmpresasContas([FromQuery] DadoDaContaPorLoginOuCPFRequest request)
        {
            if (!string.IsNullOrEmpty(request.Login))
            {
                var response = await DoInLog(async () =>
                await _empresaDapperRepository.ExecuteProcedureQueryAsync(new ProcedureRequest
                {
                    Name = "SPR_MUFG_OpenBanking_ObterEmpresasContasPorLogin",
                    Parameters = new Dictionary<string, string>
                    {
                        { "Login", request.Login }
                    }
                }),
                request,
                "obter-empresas-contas");

                return Ok(response);
            }

            if (!string.IsNullOrEmpty(request.CPF))
            {
                var response = await DoInLog(async () =>
                await _empresaDapperRepository.ExecuteProcedureQueryAsync(new ProcedureRequest
                {
                    Name = "SPR_MUFG_OpenBanking_ObterEmpresasContasPorCpf",
                    Parameters = new Dictionary<string, string>
                    {
                         { "CPF", request.CPF }
                    }
                }),
                request,
                "obter-empresas-contas");

                return Ok(response);
            }

            return Ok();
        }

        [HttpGet("obter-convenios")]
        public async Task<IActionResult> ObterConvenios([FromQuery] DadoDoConvenioRequest request)
        {
            _convenioDapperRepository.SetProvider(ProviderTypeEnum.Oracle);

            var response = await DoInLog(async () =>
                await _convenioDapperRepository.GetConvenioPorProcedureAsync("SCVAPITOKYOCONSULTACONVENIO", request),
                request,
                "obter-convenios");

            return Ok(response.To<DadoDoConvenioResponse>());
        }

        [HttpGet("obter-dados-dos-convenios")]
        public async Task<IActionResult> ObterDadosDosConvenios([FromQuery] int idUsuario)
        {
            var response = await DoInLog(async () =>
                 await _empresaDapperRepository.GetContasPorUsuarioAsync(idUsuario),
                 $"IdUsuario: {idUsuario}",
                 "obter-dados-dos-convenios"
             );

            return Ok(_mapper.Map<List<EmpresaModel>, List<DadoDoConvenioResponse>>(response.To<List<EmpresaModel>>()));
        }
    }
}
