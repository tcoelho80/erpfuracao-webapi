using System.Net;
using System.Net.Sockets;

namespace ERP.Furacao.Infrastructure.Identity.Helpers
{
    public class IPHelper
    {
        public static string GetIpAddress()
        {
            var ips = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (var ip in ips)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return string.Empty;
        }
    }
}
