using ERP.Furacao.Domain.Extensions;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ERP.Furacao.Application.DTOs
{
    public class ProcedureRequest
    {
        public string Name { get; set; }

        public Dictionary<string, string> Parameters { get; set; }

        [JsonIgnore]
        public Dictionary<string, object> ParametersProcedure
        {
            get
            {
                var param = new Dictionary<string, object>();
                this.Parameters.ExecuteForEach(p => param.Add(p.Key, p.Value));
                return param;
            }
        }
    }
}
