using Serilog.Events;
using System.Runtime.CompilerServices;

namespace ERP.Furacao.Infrastructure.Crosscutting.Interfaces.Logs
{
    public interface ILogService
    {
        void RecLog(LogEventLevel level, string message,
                    [CallerMemberName] string methodName = null,
                    string ipAddress = null, string userId = null,
                    string apiVersion = null);
    }
}
