using Serilog.Events;
using System;

namespace ERP.Furacao.Infrastructure.Crosscutting.Model.Logs
{
    public class LogModel
    {
        public LogModel(LogEventLevel level, string message, string methodName = "", string requestedBy = "", string ipAddress = "", string userId = "", string apiVersion = "")
        {
            this.Level = level;
            this.Message = message;
            this.MethodName = methodName;
            this.RequestedBy = requestedBy;
            this.IpAddress = ipAddress;
            this.UserId = userId;
            this.ApiVersion = apiVersion;
        }

        public LogEventLevel Level { get; set; }
        public string Message { get; set; }
        public string MethodName { get; set; }
        public string RequestedBy { get; set; }
        public string IpAddress { get; set; }
        public string UserId { get; set; }
        public string ApiVersion { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
