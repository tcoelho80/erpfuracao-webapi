using System;

namespace ERP.Furacao.Application.DTOs.Account
{
    public class LogServiceRequest
    {
        public Double IdLogServico { get; set; }
        public DateTime DtLog { get; set; }
        public string IdRegistro { get; set; }
        public string DsLogServico { get; set; }
        public int IdSistema { get; set; }
        public string CodLog { get; set; }
        public int InErro { get; set; }
        public string IdExecucao { get; set; }
    }
}
