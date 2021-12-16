using Newtonsoft.Json;
using System.Collections.Generic;

namespace ERP.Furacao.Application.DTOs.Log
{
    public class LogResponse : LogBaseResponse
    {
        [JsonProperty("Dados")]
        public List<logDadosResponse> LogDados { get; set; }
    }


}
