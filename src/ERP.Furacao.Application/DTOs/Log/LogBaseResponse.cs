using Newtonsoft.Json;

namespace ERP.Furacao.Application.DTOs.Log
{
    public class LogBaseResponse
    {
        [JsonProperty("CodigoRetorno")]
        public int PONCodigoRetorno { get; set; }
        [JsonProperty("MsgRetorno")]
        public string POSMsgRetorno { get; set; }
    }
}
