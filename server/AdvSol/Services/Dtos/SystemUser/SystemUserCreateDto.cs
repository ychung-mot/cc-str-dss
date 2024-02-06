using System.Text.Json.Serialization;

namespace AdvSol.Services.Dtos.SystemUser
{
    public class SystemUserCreateDto : IAddress
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string? LastName { get; set; }
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? Province { get; set; }
        public string? PostalCode { get; set; }
        public string? PhoneNumber { get; set; }
        [JsonIgnore]
        public double? Longitude { get; set; }
        [JsonIgnore]
        public double? Latitude { get; set; }
    }
}
