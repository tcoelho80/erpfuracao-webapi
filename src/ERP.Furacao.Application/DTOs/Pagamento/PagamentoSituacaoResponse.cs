using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ERP.Furacao.Application.DTOs.Pagamento
{
    public class PagamentoSituacaoResponse : PagamentoBaseResponse
    {
        [JsonProperty("Dados")]
        public List<ComprovanteResponse> POCurDados { get; set; }
    }

    public class ComprovanteResponse
    {
        [JsonProperty("CodBeneficiario")]
        public string Cod_Beneficiario { get; set; }
        [JsonProperty("TipoPessoaBeneficiario")]
        public string Tipo_Pessoa_Beneficiario { get; set; }
        [JsonProperty("NomeBeneficiario")]
        public string Nome_Beneficiario { get; set; }
        [JsonProperty("NomeFantasiaBeneficiario")]
        public string Nome_Fantasia_Beneficiario { get; set; }
        [JsonProperty("FormaPagamento")]
        public int Forma_Pagamento { get; set; }
        [JsonProperty("TipoCompromisso")]
        public int Tipo_Compromisso { get; set; }
        [JsonProperty("NumCompromisso")]
        public string Num_Compromisso { get; set; }
        [JsonProperty("ValorCompromisso")]
        public double? Valor_Compromisso { get; set; }
        [JsonProperty("ValorAutorizado")]
        public double? Valor_Autorizado { get; set; }
        [JsonProperty("ValorTitulo")]
        public double? Valor_Titulo { get; set; }
        [JsonProperty("ValorAbatimento")]
        public double? Valor_Abatimento { get; set; }
        [JsonProperty("ValorJuros")]
        public double? Valor_Juros { get; set; }
        [JsonProperty("ValorMulta")]
        public double? Valor_Multa { get; set; }
        [JsonProperty("ValorEncargos")]
        public double? Valor_Encargos { get; set; }
        [JsonProperty("ValorPago")]
        public double? Valor_Pago { get; set; }
        [JsonProperty("DataVencimento")]
        public DateTime Data_Vencimento { get; set; }
        [JsonProperty("DataPagamento")]
        public DateTime Data_Pagamento { get; set; }
        [JsonProperty("Convenio")]
        public int Convenio { get; set; }
        [JsonProperty("Autorizadores")]
        public string Autorizadores { get; set; }
        [JsonProperty("SituacaoCompromisso")]
        public int Situacao_Compromisso { get; set; }
        [JsonProperty("DescricaoSituacaoCompromisso")]
        public string Descricao_Situacao_Compromisso { get; set; }
        [JsonProperty("AgenciaDebito")]
        public int Agencia_Debito { get; set; }
        [JsonProperty("ContaDebito")]
        public int Conta_Debito { get; set; }
        [JsonProperty("DataUltimaAtualizacao")]
        public DateTime Data_Ultima_Atualizacao { get; set; }
        [JsonProperty("ValorTarifa")]
        public double? Valor_Tarifa { get; set; }
        [JsonProperty("LinhaDigitavel")]
        public string Linha_Digitavel { get; set; }
        [JsonProperty("NumeroBancoCredito")]
        public int Numero_Banco_Credito { get; set; }
        [JsonProperty("NomeBancoCredito")]
        public string Nome_Banco_Credito { get; set; }
        [JsonProperty("NumAgenciaCredito")]
        public int Num_Agencia_Credito { get; set; }
        [JsonProperty("NomeAgenciaCredito")]
        public string Nome_Agencia_Credito { get; set; }
        [JsonProperty("ContaCredito")]
        public int Conta_Credito { get; set; }
        [JsonProperty("DescricaoFinalidade")]
        public string Descricao_Finalidade { get; set; }
        [JsonProperty("NSU")]
        public string NSU { get; set; }
        [JsonProperty("TipoBoleto")]
        public int Tipo_Boleto { get; set; }
        [JsonProperty("TipoContaDebito")]
        public int Tipo_Conta_Debito { get; set; }
        [JsonProperty("NomeSacado")]
        public string Nome_Sacado { get; set; }
        [JsonProperty("CanalOrigem")]
        public string Canal_Origem { get; set; }
        [JsonProperty("IdSacado")]
        public string Id_Sacado { get; set; }
        [JsonProperty("TipoPessoaSacado")]
        public string Tipo_Pessoa_Sacado { get; set; }
        [JsonProperty("Observacoes")]
        public string Observacoes { get; set; }
        [JsonProperty("NomePagador")]
        public string Nome_Pagador { get; set; }
        [JsonProperty("CNPJCPFPagador")]
        public string CNPJ_CPF_Pagador { get; set; }
        [JsonProperty("TipoPessoaPagador")]
        public string Tipo_Pessoa_Pagador { get; set; }
        [JsonProperty("CNPJCPFPagadorFinal")]
        public string CNPJ_CPF_Pagador_Final { get; set; }
        [JsonProperty("TipoPessoaPagadorFinal")]
        public string Tipo_Pessoa_Pagador_Final { get; set; }
        [JsonProperty("HoraPagamento")]
        public string Hora_Pagamento { get; set; }
        [JsonProperty("Autenticacao")]
        public string Autenticacao { get; set; }
        [JsonProperty("RealizadoEspecie")]
        public string Realizado_Especie { get; set; }
        [JsonProperty("ComplementoErro")]
        public string Complemento_Erro { get; set; }
        [JsonProperty("UsuarioInclusao")]
        public string Usuario_Inclusao { get; set; }
        [JsonProperty("IdPagamento")]
        public Int64 Id_Pagamento { get; set; }
    }
}
