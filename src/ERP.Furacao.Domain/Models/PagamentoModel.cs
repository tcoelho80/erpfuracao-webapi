using System;

namespace ERP.Furacao.Domain.Models
{
    public class PagamentoModel
    {
        public int Convenio { get; set; }
        public Int64 CodigoBeneficiario { get; set; }
        public string TipoBeneficiario { get; set; }
        public string TipoCompromisso { get; set; }
        public string NumeroCompromisso { get; set; }
        public string CodFinalidadeDoc { get; set; }
        public double ValorCompromisso { get; set; }
        public string FormaPagamento { get; set; }
        public int BancoCredito { get; set; }
        public int AgenciaCredito { get; set; }
        public string ContaCredito { get; set; }
        public DateTime DataPagamento { get; set; }
        public string NomeBeneficiario { get; set; }
        public string TipoTransferencia { get; set; }
        public string Responsavel { get; set; }
        public DateTime? DataVencimento { get; set; }
        public string LinhaDigitavel { get; set; }
        public string TipoCodigoBarras { get; set; }
        public string TipoContaDestino { get; set; }
        public string Email { get; set; }
        public string NomeSacado { get; set; }
        public string TipoBoleto { get; set; }
        public double? ValorAbatimento { get; set; }
        public double? ValorJuros { get; set; }
        public string DNascEAprovado { get; set; }
        public Int64 CodigoSacado { get; set; }
        public string TipoPessoaSacado { get; set; }
        public string Observacoes { get; set; }
    }
}
