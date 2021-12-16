using Dapper.Oracle;
using ERP.Furacao.Domain.Attributes;
using Newtonsoft.Json;
using System.Data;

namespace ERP.Furacao.Application.DTOs.Procedure
{
    public class OutputBase
    {
        [JsonIgnore]
        [OracleParameter(Direction = ParameterDirection.Output, Name = "PONCODIGORETORNO", Type = OracleMappingType.Int32)]
        public int CodigoRetorno { get; set; }

        [JsonIgnore]
        [OracleParameter(Direction = ParameterDirection.Output, Name = "POSMSGRETORNO", Type = OracleMappingType.NVarchar2, Size = 500)]
        public string MsgRetorno { get; set; }
    }
}
