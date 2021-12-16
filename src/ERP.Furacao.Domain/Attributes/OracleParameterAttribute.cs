using Dapper.Oracle;
using System;
using System.Data;

namespace ERP.Furacao.Domain.Attributes
{
    public class OracleParameterAttribute : Attribute
    {
        public string Name { get; set; }
        public OracleMappingType Type { get; set; }
        public ParameterDirection Direction { get; set; }
        public int Size { get; set; }
    }
}
