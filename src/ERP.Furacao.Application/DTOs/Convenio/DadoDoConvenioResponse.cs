using System.Collections.Generic;

namespace ERP.Furacao.Application.DTOs.Convenio
{
    public class DadoDoConvenioResponse
    {
        public List<int> IdsConvenios { get; set; } = new List<int>();
        public int CodigoRetorno { get; set; }
        public string MensagemRetorno { get; set; }
    }
}
