using System.Collections.Generic;

namespace ERP.Furacao.Application.DTOs.Convenio
{
    public class DadoDoConvenioAgenciaContaRequest
    {
        public Dictionary<string, string> Inputs { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> Outputs { get; set; } = new Dictionary<string, string>();
        public string Cursor { get; set; }
    }
}
