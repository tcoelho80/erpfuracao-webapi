using Newtonsoft.Json;

namespace ERP.Furacao.Application.DTOs.Log
{
    public class logDadosResponse
    {
        [JsonProperty("Level")]
        public string Level { get; set; }



        [JsonProperty("MethodName")]
        public string MethodName { get; set; }

        [JsonProperty("RequestedBy")]
        public string RequestedBy { get; set; }

        [JsonProperty("IpAddress")]
        public string IpAddress { get; set; }

        [JsonProperty("UserId")]
        public string UserId { get; set; }

        [JsonProperty("ApiVersion")]
        public string ApiVersion { get; set; }

        [JsonProperty("CreatedDate")]
        public string CreatedDate { get; set; }


    }
}
