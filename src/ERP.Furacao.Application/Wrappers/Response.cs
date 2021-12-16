using System.Collections.Generic;

namespace ERP.Furacao.Application.Wrappers
{
    public class Response<T>
    {
        public Response()
        {
        }

        public Response(T data, string message = null)
        {
            Succeeded = true;
            Message = message;
            Data = data;
        }

        public Response(T data, string message = null, string logEventLevel = null)
        {
            Succeeded = true;
            Message = message;
            Data = data;
            LogEventLevel = logEventLevel;
        }

        public Response(string message)
        {
            Succeeded = false;
            Message = message;
        }

        public Response(string message, string logEventLevel)
        {
            Succeeded = false;
            Message = message;
            LogEventLevel = logEventLevel;
        }

        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
        public T Data { get; set; }
        public string LogEventLevel { get; set; } = "Information";
    }
}
