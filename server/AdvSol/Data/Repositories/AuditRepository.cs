using AdvSol.Data.Entities;
using AdvSol.Services.Dtos;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AdvSol.Data.Repositories
{
    public interface IAuditRepository : IRepositoryBase<Audit>
    {
        void CreateAudits<T>(T entity) where T : class;
        Task<Audit[]> GetAuditRecords(string entity, int id);
    }
    public class AuditRepository : RepositoryBase<Audit>, IAuditRepository
    {
        public AuditRepository(AppDbContext dbContext, IMapper mapper, ICurrentUser currentUser)
            : base(dbContext, mapper, currentUser)
        {
        }

        public void CreateAudits<T>(T entity) where T : class
        {
            DbContext.CreateAuditEntryForAdd(DbContext.Entry(entity));
        }

        public async Task<Audit[]> GetAuditRecords(string entity, int id)
        {
            var audits = await DbSet.AsNoTracking()
                .Where(x => x.Entity.ToLower() == entity.ToLower() && x.EntityId == id)
                .OrderBy(x => x.UpdatedTime)
                .ThenBy(x => x.Field)
                .ToArrayAsync();

            return audits;
        }
    }
}