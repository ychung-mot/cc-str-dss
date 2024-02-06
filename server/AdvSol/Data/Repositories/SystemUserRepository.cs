using AdvSol.Data.Entities;
using AdvSol.Services.Dtos;
using AdvSol.Services.Dtos.SystemUser;
using AdvSol.Utils;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AdvSol.Data.Repositories
{
    public interface ISystemUserRepository : IRepositoryBase<SystemUser>
    {
        Task<SystemUserDto[]> GetSystemUsersAsync();
        Task<SystemUserDto?> GetSystemUserAsync(int id);
        Task<SystemUserDto?> GetSystemUserByUsernameAsync(string username);

        Task<SystemUser> CreateSystemUserAsync(SystemUserDto dto);
        Task<bool> CheckCredentials(CredentialDto dto);
        Task<SystemUserDto?> GetSystemUserByUsernameOrLastNameAsync(string userName, string lastName);
        Task<int> GetNumberOfApplicantsAsync();
        Task<SystemUser[]> GetAdminUsers();
    }
    public class SystemUserRepository : RepositoryBase<SystemUser>, ISystemUserRepository
    {
        public SystemUserRepository(AppDbContext dbContext, IMapper mapper, ICurrentUser currentUser)
            : base(dbContext, mapper, currentUser)
        {
        }

        public async Task<SystemUserDto[]> GetSystemUsersAsync()
        {
            var users = await DbSet.AsNoTracking().ToArrayAsync();

            return Mapper.Map<SystemUserDto[]>(users);
        }
        public async Task<int> GetNumberOfApplicantsAsync()
        {
            var count = await DbSet.AsNoTracking().CountAsync(x => x.Role.CodeSet == "ROLE" && x.Role.CodeValue == Roles.Applicant);

            return count;
        }

        public async Task<SystemUserDto?> GetSystemUserAsync(int id)
        {
            var user = await DbSet.AsNoTracking()
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
                return null;

            return Mapper.Map<SystemUserDto>(user);
        }

        public async Task<SystemUserDto?> GetSystemUserByUsernameAsync(string username)
        {
            var user = await DbSet.AsNoTracking()
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Username == username);

            if (user == null)
                return null;

            return Mapper.Map<SystemUserDto>(user);
        }


        public async Task<SystemUserDto?> GetSystemUserByUsernameOrLastNameAsync(string userName, string lastName)
        {
            var user = await DbSet.AsNoTracking()
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Username == userName || x.LastName == lastName);

            if (user == null)
                return null;

            return Mapper.Map<SystemUserDto>(user);
        }

        public async Task<SystemUser> CreateSystemUserAsync(SystemUserDto dto)
        {
            var entity = Mapper.Map<SystemUser>(dto);

            await DbContext.AddAsync(entity);

            return entity;
        }

        public async Task<bool> CheckCredentials(CredentialDto dto)
        {
            var pwdHashed = dto.Password.ComputeSHA256Hash();
            var entity = await DbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Username == dto.Username && x.Password == pwdHashed);
            return entity != null;
        }

        public async Task<SystemUser[]> GetAdminUsers()
        {
            return await DbSet.AsNoTracking()
                .Where(x => x.Role.CodeValue == Roles.Admin)
                .ToArrayAsync();
        }
    }
}