using AutoMapper;
using ERP.Furacao.Application.Wrappers;
using ERP.Furacao.Infrastructure.Crosscutting.Interfaces.Logs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Serilog.Events;
using System;
using System.Threading.Tasks;

namespace ERP.Furacao.WebApi.Controllers
{
    [ApiController]
    [Route("/internal/api/v{version:apiVersion}/[controller]")]
    public class BaseController : ControllerBase
    {
        public readonly IMapper _mapper;
        public readonly ILogService _logService;
        private IMediator _mediator;
        public string IpAddress => Request.Headers.ContainsKey("X-Forwarded-For") ?
                                    Request.Headers["X-Forwarded-For"] :
                                    HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();

        public string ApiVersion => Request?.RouteValues["version"].ToString();

        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        public BaseController()
        {

        }

        public BaseController(IMapper mapper)
        {
            _mapper = mapper;
        }

        public BaseController(ILogService logService)
        {
            _logService = logService;
        }

        public BaseController(IMapper mapper, ILogService logService)
        {
            _mapper = mapper;
            _logService = logService;
        }

        protected async Task<object> DoInLog(Func<Task<object>> code, object request, string methodName = null, string userId = null)
        {
            try
            {
                _logService.RecLog(LogEventLevel.Information, message: $"Request: {JsonConvert.SerializeObject(request)}", methodName, IpAddress, userId, ApiVersion);

                var response = await code();

                _logService.RecLog(LogEventLevel.Information, message: $"Response: {JsonConvert.SerializeObject(response)}", methodName, IpAddress, userId, ApiVersion);

                return response;
            }
            catch (Exception ex)
            {
                _logService.RecLog(LogEventLevel.Error, ex.Message, methodName, IpAddress, userId, ApiVersion);
                return new Response<string>(ex.Message, "Error");
            }
        }

    }
}
