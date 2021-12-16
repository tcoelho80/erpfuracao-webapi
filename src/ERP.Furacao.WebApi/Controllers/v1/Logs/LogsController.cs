using AutoMapper;
using ERP.Furacao.Application.Repositories;
using ERP.Furacao.Domain.Models;
using ERP.Furacao.Infrastructure.Crosscutting.Interfaces.Logs;
using ERP.Furacao.Infrastructure.Data.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ERP.Furacao.WebApi.Controllers.v1.Logs
{
    /// <summary>
    /// Conta Controller
    /// </summary>
    [Authorize]
    [ApiVersion("1.0")]
    [ControllerName("Logs")]
    public class LogsController : BaseController
    {
        private readonly ILogDapperRepository _logDapperRepository;

        public LogsController(ILogDapperRepository logDapperRepository,
                                    ILogService logService,
                                    IMapper mapper)
            : base(mapper, logService)
        {
            _logDapperRepository = logDapperRepository;
            _logDapperRepository.SetProvider(ProviderTypeEnum.SqlServer);
        }




        [AllowAnonymous]
        [HttpGet("obter-logs")]
        public IActionResult ObterLogs([FromQuery] LogModel request)
        {
            var response = _logDapperRepository.GetLogs(request);

            return Ok(JsonConvert.SerializeObject(response, Formatting.Indented));
        }


    }
}
