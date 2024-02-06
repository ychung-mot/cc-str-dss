namespace AdvSol.Data.Entities
{
    public class StrApplication
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
        public virtual CommonCode ZoningType { get; set; }
        public virtual CommonCode StrAffiliate { get; set; }
        public virtual CommonCode ComplianceStatus { get; set; }
        public virtual SystemUser Applicant { get; set; }        
    }
}
