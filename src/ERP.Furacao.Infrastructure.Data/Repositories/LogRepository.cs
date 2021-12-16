using ERP.Furacao.Application.DTOs.Log;
using ERP.Furacao.Application.Repositories;
using ERP.Furacao.Domain.Models;
using ERP.Furacao.Infrastructure.Data.Contexts;
using ERP.Furacao.Infrastructure.Data.Settings;
using Infrastructure.Data.Repositories;
using System.Collections.Generic;


namespace ERP.Furacao.Infrastructure.Data.Repositories
{
    public class LogRepository : EntityBaseRepository<LogModel>, ILogRepository
    {
        private readonly ApplicationContext _context;
        public LogRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }
    }

    public class LogDapperRepository : DapperBaseRepository<LogModel>, ILogDapperRepository
    {
        public LogDapperRepository(DapperDbSettings dbSettings) : base(dbSettings)
        {

        }

        public LogResponse GetLogs(LogModel filter)
        {


            var parametersIn = new Dictionary<string, object>();

            parametersIn.Add("@createDate", filter.CreatedDate.Date.ToString("yyyy-MM-dd"));
            parametersIn.Add("@ApiVersion", filter.ApiVersion);
            parametersIn.Add("@IpAddress", filter.IpAddress);
            parametersIn.Add("@Level", filter.Level);
            parametersIn.Add("@Message", filter.Message);
            parametersIn.Add("@MethodName", filter.MethodName);
            parametersIn.Add("@RequestedBy", filter.RequestedBy);

            var query = $@"SELECT 
                            LEVEL, 
                            MESSAGE, 
                            METHODNAME, 
                            REQUESTEDBY, 
                            CREATEDDATE, 
                            IPADDRESS, 
                            APIVERSION 
                           FROM LOGGING.LOG 
                           WHERE CONVERT(VARCHAR, CREATEDDATE, 23) = @createDate
                           {(string.IsNullOrEmpty(filter.ApiVersion) ? string.Empty : $" AND UPPER(APIVERSION) LIKE UPPER('%@ApiVersion%')") }
                           {(string.IsNullOrEmpty(filter.IpAddress) ? string.Empty : $" AND UPPER(IPADDRESS) LIKE UPPER('%@IpAddress%')") }
                           {(string.IsNullOrEmpty(filter.Level) ? string.Empty : $" AND UPPER(LEVEL) LIKE UPPER('%@Level%')") }
                           {(string.IsNullOrEmpty(filter.Message) ? string.Empty : $" AND UPPER(MESSAGE) LIKE UPPER('%@Message%')") }
                           {(string.IsNullOrEmpty(filter.MethodName) ? string.Empty : $" AND UPPER(METHODNAME) LIKE UPPER('%@MethodName%')") }
                           {(string.IsNullOrEmpty(filter.RequestedBy) ? string.Empty : $" AND UPPER(REQUESTEDBY) LIKE UPPER('%@RequestedBy%')") }
                           ORDER BY CREATEDDATE 
            DESC";

            var dados = base.GetExecuteQuery(query, parametersIn);



            List<logDadosResponse> logsDadosResponse = new List<logDadosResponse>();

            foreach (var item in dados)
            {
                logDadosResponse logDadosResponse = new logDadosResponse();
                logDadosResponse.Level = item.Level;

                logDadosResponse.MethodName = item.MethodName;
                logDadosResponse.RequestedBy = item.RequestedBy;
                logDadosResponse.IpAddress = item.IpAddress;
                logDadosResponse.UserId = item.UserId;
                logDadosResponse.ApiVersion = item.ApiVersion;
                logDadosResponse.CreatedDate = item.CreatedDate.ToString();

                logsDadosResponse.Add(logDadosResponse);
            }




            return new LogResponse
            {
                PONCodigoRetorno = 777,
                POSMsgRetorno = "Dados OK",
                LogDados = logsDadosResponse
            };

        }
    }
}
