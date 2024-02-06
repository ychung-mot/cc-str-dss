using System.Text.Json.Serialization;

namespace AdvSol.Services.Dtos.CommonCode
{
    public class CommonCodeDto
    {
        [JsonPropertyName("value")]
        public int Id { get; set; }
        public string CodeSet { get; set; }
        [JsonPropertyName("label")]
        public string CodeName { get; set; }
        public string CodeValue { get; set; }
    }
}
