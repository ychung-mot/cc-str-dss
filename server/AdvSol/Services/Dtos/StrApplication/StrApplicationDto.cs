using AdvSol.Services.Dtos.CommonCode;
using AdvSol.Services.Dtos.SystemUser;

namespace AdvSol.Services.Dtos.StrApplication
{
    public class StrApplicationDto : IAddress
    {
        public int Id { get; set; }
        public int ApplicantId { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public int ZoningTypeId { get; set; }
        public decimal SquareFootage { get; set; }
        public int StrAffiliateId { get; set; }
        public bool IsOwnerPrimaryResidence { get; set; }
        public int ComplianceStatusId { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public DateTime DateCreated { get; set; }
        public virtual CommonCodeDto ZoningType { get; set; }
        public virtual CommonCodeDto StrAffiliate { get; set; }
        public virtual CommonCodeDto ComplianceStatus { get; set; }
        public virtual SystemUserDto Applicant { get; set; }
    }
}
