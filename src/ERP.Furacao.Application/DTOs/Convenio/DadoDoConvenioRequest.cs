using Dapper.Oracle;
using ERP.Furacao.Application.DTOs.Procedure;
using ERP.Furacao.Domain.Attributes;
using Newtonsoft.Json;
using System.Data;

namespace ERP.Furacao.Application.DTOs.Convenio
{
    public class DadoDoConvenioRequest : OutputBase
    {
        #region Inputs
        [OracleParameter(Direction = ParameterDirection.Input, Name = "PINAGENCIA", Type = OracleMappingType.Int32)]
        public int Agencia { get; set; }

        [OracleParameter(Direction = ParameterDirection.Input, Name = "PISCONTA", Type = OracleMappingType.Int32)]
        public int Conta { get; set; }
        #endregion

        #region Outputs
        [JsonIgnore]
        [OracleParameter(Direction = ParameterDirection.Output, Name = "POCURDADOS", Type = OracleMappingType.RefCursor)]
        public object Dados { get; set; }
        #endregion
    }
}
