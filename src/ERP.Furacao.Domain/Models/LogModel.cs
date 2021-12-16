using System;

namespace ERP.Furacao.Domain.Models
{
    public class LogModel
    {
        public string Level { get; set; }
        public string Message { get; set; }
        public string MethodName { get; set; }
        public string RequestedBy { get; set; }
        public string IpAddress { get; set; }
        public string UserId { get; set; }
        public string ApiVersion { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
