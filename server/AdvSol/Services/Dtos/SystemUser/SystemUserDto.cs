using AdvSol.Services.Dtos.CommonCode;

namespace AdvSol.Services.Dtos.SystemUser
{
    public class SystemUserDto : IAddress
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string? LastName { get; set; }
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? Province { get; set; }
        public string? PostalCode { get; set; }
        public string? PhoneNumber { get; set; }
        public int RoleId { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public DateTime DateCreated { get; set; }
        public CommonCodeDto? Role { get; set; }
    }
}
