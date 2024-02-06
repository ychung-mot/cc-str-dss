using AdvSol.Data.Entities;
using AdvSol.Utils;

namespace AdvSol.Data
{
    public static class SeedData
    {
        public static void Initialize(AppDbContext context)
        {
            if (!context.CommonCodes.Any())
                context.CommonCodes.AddRange(SeedData.CommonCodes);

            if (!context.SystemUsers.Any())
                context.SystemUsers.AddRange(SeedData.SystemUsers);

            context.SaveChanges();
        }
        public static List<SystemUser> SystemUsers { get; } = new List<SystemUser>
        {
            new SystemUser { Username = "ADMIN1", Password = "9f86d081884c7d659a2feaa0c55ad015a3bf4f1b2b0b822cd15d6c15b0f00a08", LastName = "Admin 1", RoleId = 1, DateCreated = DateTime.UtcNow },
            new SystemUser { Username = "ADMIN2", Password = "9f86d081884c7d659a2feaa0c55ad015a3bf4f1b2b0b822cd15d6c15b0f00a08", LastName = "Admin 2", RoleId = 1, DateCreated = DateTime.UtcNow },
        };

        public static List<CommonCode> CommonCodes { get; } = new List<CommonCode>
        {
            new CommonCode { Id = 1, CodeSet = "ROLE", CodeName = Roles.Admin, CodeValue = Roles.Admin },
            new CommonCode { Id = 2, CodeSet = "ROLE", CodeName = Roles.Applicant, CodeValue = Roles.Applicant },

            new CommonCode { Id = 3, CodeSet = "ZONE_TYPE", CodeName = "Residential", CodeValue = "Residential" },
            new CommonCode { Id = 4, CodeSet = "ZONE_TYPE", CodeName = "Commercial", CodeValue = "Commercial" },
            new CommonCode { Id = 5, CodeSet = "ZONE_TYPE", CodeName = "Mixed Use", CodeValue = "MixedUse" },

            new CommonCode { Id = 6, CodeSet = "STR_AFFILIATE", CodeName = "AirBNB", CodeValue = "AirBNB" },
            new CommonCode { Id = 7, CodeSet = "STR_AFFILIATE", CodeName = "VRBO", CodeValue = "VRBO" },

            new CommonCode { Id = 8, CodeSet = "COMPLIANCE_STATUS", CodeName = "Pending", CodeValue = "Pending" },
            new CommonCode { Id = 9, CodeSet = "COMPLIANCE_STATUS", CodeName = "Compliant", CodeValue = "Compliant" },
            new CommonCode { Id = 10, CodeSet = "COMPLIANCE_STATUS", CodeName = "Non-compliant", CodeValue = "Non-compliant" },
        };
    }
}
