using AdvSol.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AdvSol.Data
{
    public partial class AppDbContext : DbContext
    {
        public DbSet<Audit> Audits { get; set; }
        public DbSet<CommonCode> CommonCodes { get; set; }
        public DbSet<StrApplication> StrApplications { get; set; }
        public DbSet<SystemUser> SystemUsers { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
