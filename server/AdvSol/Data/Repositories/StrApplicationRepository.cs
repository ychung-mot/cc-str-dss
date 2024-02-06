using AdvSol.Data.Entities;
using AdvSol.Services.Dtos;
using AdvSol.Services.Dtos.StrApplication;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AdvSol.Data.Repositories
{
    public interface IStrApplicationRepository : IRepositoryBase<StrApplication>
    {
        Task<StrApplicationDto[]> GetStrApplicationsAsync();
        Task<StrApplicationDto[]> GetStrApplicationsByApplicantAsync(int applicantId);
        Task<StrApplicationDto?> GetStrApplicationAsync(int id);
        Task<StrApplication> CreateStrApplicationAsync(StrApplicationDto dto);
        Task<int> GetNumberOfStrApplicationsByApplicantAsync(int applicantId);
        Task UpdateStrApplicationAsync(StrApplicationDto dto);
    }
    public class StrApplicationRepository : RepositoryBase<StrApplication>, IStrApplicationRepository
    {
        public StrApplicationRepository(AppDbContext dbContext, IMapper mapper, ICurrentUser currentUser)
            : base(dbContext, mapper, currentUser)
        {
        }

        public async Task<StrApplicationDto[]> GetStrApplicationsAsync()
        {
            var applications = await DbSet.AsNoTracking()
                .Include(x => x.Applicant)
                .Include(x => x.ZoningType)
                .Include(x => x.ComplianceStatus)
                .Include(x => x.StrAffiliate)
                .ToArrayAsync();

            return Mapper.Map<StrApplicationDto[]>(applications);
        }

        public async Task<StrApplicationDto[]> GetStrApplicationsByApplicantAsync(int applicantId)
        {
            var applications = await DbSet.AsNoTracking()
                .Include(x => x.Applicant)
                .Include(x => x.ZoningType)
                .Include(x => x.ComplianceStatus)
                .Include(x => x.StrAffiliate)
                .Where(x => x.ApplicantId == applicantId)
                .ToArrayAsync();

            return Mapper.Map<StrApplicationDto[]>(applications);
        }

        public async Task<StrApplicationDto?> GetStrApplicationAsync(int id)
        {
            var application = await DbSet.AsNoTracking()
                .Include(x => x.Applicant)
                .Include(x => x.ZoningType)
                .Include(x => x.ComplianceStatus)
                .Include(x => x.StrAffiliate)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (application == null)
                return null;

            return Mapper.Map<StrApplicationDto>(application);
        }

        public async Task<StrApplication> CreateStrApplicationAsync(StrApplicationDto dto)
        {
            var entity = Mapper.Map<StrApplication>(dto);

            await DbContext.AddAsync(entity);

            return entity;
        }

        public async Task<int> GetNumberOfStrApplicationsByApplicantAsync(int applicantId)
        {
            return await DbSet.CountAsync(x => x.ApplicantId == applicantId);            
        }

        public async Task UpdateStrApplicationAsync(StrApplicationDto dto)
        {
            var entity = await DbSet.FirstAsync(x => x.Id == dto.Id);

            Mapper.Map(dto, entity);
        }
    }
}