using Dapper.Oracle;
using ERP.Furacao.Application.DTOs.Procedure;
using ERP.Furacao.Domain.Attributes;
using Newtonsoft.Json;
using System;
using System.Data;

namespace ERP.Furacao.Application.DTOs.Pagamento
{
    public class PagamentoSituacaoRequest : OutputBase
    {
        #region Inputs
        [OracleParameter(Direction = ParameterDirection.Input, Name = "PINAGENCIA", Type = OracleMappingType.Int32)]
        public int Agencia { get; set; }
        [OracleParameter(Direction = ParameterDirection.Input, Name = "PINNUMCONTA", Type = OracleMappingType.Int32)]
        public int Conta { get; set; }
        [OracleParameter(Direction = ParameterDirection.Input, Name = "PITBSITUACOESCOMPROMISSO", Type = OracleMappingType.NVarchar2)]
        public string SituacaoCompromisso { get; set; }
        [OracleParameter(Direction = ParameterDirection.Input, Name = "PITBCONVENIOS", Type = OracleMappingType.NVarchar2)]
        public string Convenio { get; set; }
        [OracleParameter(Direction = ParameterDirection.Input, Name = "PITBPAGAMENTOS", Type = OracleMappingType.NVarchar2)]
        public string Pagamento { get; set; }
        [OracleParameter(Direction = ParameterDirection.Input, Name = "PIDTDATAPAGAMENTO", Type = OracleMappingType.Date)]
        public DateTime? DataPagamento { get; set; }
        [OracleParameter(Direction = ParameterDirection.Input, Name = "PITBFORMAPAGAMENTO", Type = OracleMappingType.NVarchar2)]
        public string FormaPagamento { get; set; }
        #endregion

        #region Outputs
        [JsonIgnore]
        [OracleParameter(Direction = ParameterDirection.Output, Name = "POCURDADOS", Type = OracleMappingType.RefCursor)]
        public object Dados { get; set; }
        #endregion
    }
}
