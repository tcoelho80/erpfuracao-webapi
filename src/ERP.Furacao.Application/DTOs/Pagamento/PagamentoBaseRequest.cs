using Dapper.Oracle;
using ERP.Furacao.Application.DTOs.Procedure;
using ERP.Furacao.Domain.Attributes;
using Newtonsoft.Json;
using System;
using System.Data;

namespace ERP.Furacao.Application.DTOs.Pagamento
{
    public class PagamentoBaseRequest : OutputBase
    {
        #region Inputs
        [OracleParameter(Direction = ParameterDirection.Input, Name = "PINCONVENIO", Type = OracleMappingType.Int32)]
        public int Convenio { get; set; }

        [OracleParameter(Direction = ParameterDirection.Input, Name = "PINCODIGOBENEFICIARIO", Type = OracleMappingType.Int64)]
        public Int64 CodigoBeneficiario { get; set; }

        [OracleParameter(Direction = ParameterDirection.Input, Name = "PISTIPOBENEFICIARIO", Type = OracleMappingType.NVarchar2)]
        public string TipoBeneficiario { get; set; }

        [OracleParameter(Direction = ParameterDirection.Input, Name = "PISTIPOCOMPROMISSO", Type = OracleMappingType.NVarchar2)]
        public string TipoCompromisso { get; set; }

        [OracleParameter(Direction = ParameterDirection.Input, Name = "PISNUMEROCOMPROMISSO", Type = OracleMappingType.NVarchar2)]
        public string NumeroCompromisso { get; set; }

        [OracleParameter(Direction = ParameterDirection.Input, Name = "PISCODFINALIDADEDOC", Type = OracleMappingType.NVarchar2)]
        public string CodFinalidadeDoc { get; set; }

        [OracleParameter(Direction = ParameterDirection.Input, Name = "PIFVALORCOMPROMISSO", Type = OracleMappingType.Double)]
        public double ValorCompromisso { get; set; }

        [OracleParameter(Direction = ParameterDirection.Input, Name = "PINFORMAPAGAMENTO", Type = OracleMappingType.NVarchar2)]
        public string FormaPagamento { get; set; }

        [OracleParameter(Direction = ParameterDirection.Input, Name = "PINBANCOCREDITO", Type = OracleMappingType.Int32)]
        public int BancoCredito { get; set; }

        [OracleParameter(Direction = ParameterDirection.Input, Name = "PINAGENCIACREDITO", Type = OracleMappingType.Int32)]
        public int AgenciaCredito { get; set; }

        [OracleParameter(Direction = ParameterDirection.Input, Name = "PISCONTACREDITO", Type = OracleMappingType.NVarchar2)]
        public string ContaCredito { get; set; }

        [OracleParameter(Direction = ParameterDirection.Input, Name = "PIDTDATAPAGAMENTO", Type = OracleMappingType.Date)]
        public DateTime DataPagamento { get; set; }

        [OracleParameter(Direction = ParameterDirection.Input, Name = "PISNOMEBENEFICIARIO", Type = OracleMappingType.NVarchar2)]
        public string NomeBeneficiario { get; set; }

        [OracleParameter(Direction = ParameterDirection.Input, Name = "PISTIPOTRANSFERENCIA", Type = OracleMappingType.NVarchar2)]
        public string TipoTransferencia { get; set; }

        [OracleParameter(Direction = ParameterDirection.Input, Name = "PISRESPONSAVEL", Type = OracleMappingType.NVarchar2)]
        public string Responsavel { get; set; }

        [OracleParameter(Direction = ParameterDirection.Input, Name = "PIDTDATAVENCIMENTO", Type = OracleMappingType.Date)]
        public DateTime? DataVencimento { get; set; }

        [OracleParameter(Direction = ParameterDirection.Input, Name = "PISLINHADIGITAVEL", Type = OracleMappingType.NVarchar2)]
        public string LinhaDigitavel { get; set; }

        [OracleParameter(Direction = ParameterDirection.Input, Name = "PISTIPOCODIGOBARRAS", Type = OracleMappingType.NVarchar2)]
        public string TipoCodigoBarras { get; set; }

        [OracleParameter(Direction = ParameterDirection.Input, Name = "PISTIPOCONTADESTINO", Type = OracleMappingType.NVarchar2)]
        public string TipoContaDestino { get; set; }

        [OracleParameter(Direction = ParameterDirection.Input, Name = "PISEMAIL", Type = OracleMappingType.NVarchar2)]
        public string Email { get; set; }

        [OracleParameter(Direction = ParameterDirection.Input, Name = "PISNOMESACADO", Type = OracleMappingType.NVarchar2)]
        public string NomeSacado { get; set; }

        [OracleParameter(Direction = ParameterDirection.Input, Name = "PINTIPOBOLETO", Type = OracleMappingType.NVarchar2)]
        public string TipoBoleto { get; set; }

        [OracleParameter(Direction = ParameterDirection.Input, Name = "PINVALORABATIMENTO", Type = OracleMappingType.Double)]
        public double? ValorAbatimento { get; set; }

        [OracleParameter(Direction = ParameterDirection.Input, Name = "PINVALORJUROS", Type = OracleMappingType.Double)]
        public double? ValorJuros { get; set; }

        [OracleParameter(Direction = ParameterDirection.Input, Name = "PINDNASCEAPROVADO", Type = OracleMappingType.NVarchar2)]
        public string DNascEAprovado { get; set; }

        [OracleParameter(Direction = ParameterDirection.Input, Name = "PINCODIGOSACADO", Type = OracleMappingType.Int64)]
        public Int64 CodigoSacado { get; set; }

        [OracleParameter(Direction = ParameterDirection.Input, Name = "PINTIPOPESSOASACADO", Type = OracleMappingType.NVarchar2)]
        public string TipoPessoaSacado { get; set; }

        [OracleParameter(Direction = ParameterDirection.Input, Name = "PINOBSERVACOES", Type = OracleMappingType.NVarchar2)]
        public string Observacoes { get; set; }
        #endregion

        #region Outputs
        [JsonIgnore]
        [OracleParameter(Direction = ParameterDirection.Output, Name = "PONIDPAGAMENTO", Type = OracleMappingType.Int64)]
        public Int64 IdPagamento { get; set; }
        #endregion
    }
}
