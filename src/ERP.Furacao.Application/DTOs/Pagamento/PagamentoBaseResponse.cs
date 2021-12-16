using Newtonsoft.Json;

namespace ERP.Furacao.Application.DTOs.Pagamento
{
    public class PagamentoBaseResponse
    {
        [JsonProperty("CodigoRetorno")]
        public int PONCodigoRetorno { get; set; }
        [JsonProperty("MsgRetorno")]
        public string POSMsgRetorno { get; set; }
    }
}
