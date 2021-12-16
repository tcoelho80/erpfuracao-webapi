using ERP.Furacao.Infrastructure.Crosscutting.Interfaces.Logs;
using Serilog;
using Serilog.Events;
using System;
using System.Runtime.CompilerServices;

namespace ERP.Furacao.Infrastructure.Crosscutting.Services.Logs
{
    public class LogService : ILogService
    {
        public void RecLog(LogEventLevel level, string message, [CallerMemberName] string methodName = null, string ipAddress = null, string userId = null, string apiVersion = null)
        {
            var template = "{Message}{Level}{MethodName}{RequestedBy}{IpAddress}{UserId}{ApiVersion}{CreatedDate}";
            switch (level)
            {
                case LogEventLevel.Information:
                    Log.Information(template, message, level, methodName, Environment.MachineName, ipAddress, userId, apiVersion, DateTime.UtcNow);
                    break;
                case LogEventLevel.Debug:
                    Log.Debug(template, message, level, methodName, Environment.MachineName, ipAddress, userId, apiVersion, DateTime.UtcNow);
                    break;
                case LogEventLevel.Error:
                    Log.Error(template, message, level, methodName, Environment.MachineName, ipAddress, userId, apiVersion, DateTime.UtcNow);
                    break;
                case LogEventLevel.Warning:
                    Log.Warning(template, message, level, methodName, Environment.MachineName, ipAddress, userId, apiVersion, DateTime.UtcNow);
                    break;
            }
        }

    }
}
