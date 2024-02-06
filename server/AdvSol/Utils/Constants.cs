namespace AdvSol.Utils
{
    public class Constants
    {
        public static DateTime MaxDate = new DateTime(9999, 12, 31);
        public static DateTime MinDate = new DateTime(1900, 1, 1);
        public const string VancouverTimeZone = "America/Vancouver";
        public const string PacificTimeZone = "Pacific Standard Time";
    }
    public static class Entities
    {
        public const string SystemUser = "SystemUser";
        public const string StrApplication = "StrApplication";
        public const string Audit = "Audit";
    }
    public static class Fields
    {
        public const string StreetAddress = "StreetAddress";
        public const string City = "City";
        public const string Province = "Province";
        public const string PostalCode = "PostalCode";
        public const string ZoningTypeId = "ZoningTypeId";
        public const string SquareFootage = "SquareFootage";
        public const string StrAffiliateId = "StrAffiliateId";
        public const string IsOwnerPrimaryResidence = "IsOwnerPrimaryResidence";
        public const string ComplianceStatusId = "ComplianceStatusId";

        public const string Username = "Username";
        public const string Passwrod = "Passwrod";
        public const string LastName = "LastName";
        public const string PhoneNumber = "PhoneNumber";
        public const string RoleId = "RoleId";

    }
    public static class FieldTypes
    {
        public const string String = "S";
        public const string Decimal = "N";
        public const string Date = "D";
    }

    public static class Roles
    {
        public const string Admin = "Admin";
        public const string Applicant = "Applicant";
    }

    public static class CodeSet
    {
        public const string Role = "ROLE";
        public const string ZoneType = "ZONE_TYPE";
        public const string StrAffiliate = "STR_AFFILIATE";
        public const string ComplianceStatus = "COMPLIANCE_STATUS";
    }
}
