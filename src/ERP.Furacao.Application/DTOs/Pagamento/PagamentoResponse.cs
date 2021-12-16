using Newtonsoft.Json;
using System;

namespace ERP.Furacao.Application.DTOs.Pagamento
{
    public class PagamentoResponse : PagamentoBaseResponse
    {
        [JsonProperty("IdPagamento")]
        public Int64 IdPagamento { get; set; }
    }
}
