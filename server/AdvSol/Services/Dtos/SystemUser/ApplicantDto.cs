using AdvSol.Services.Dtos.CommonCode;

namespace AdvSol.Services.Dtos.SystemUser
{
    public class ApplicantDto
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public CommonCodeDto Role { get; set; }
    }
}
