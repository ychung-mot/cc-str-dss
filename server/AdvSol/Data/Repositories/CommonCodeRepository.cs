using AdvSol.Data.Entities;
using AdvSol.Services.Dtos;
using AdvSol.Services.Dtos.CommonCode;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AdvSol.Data.Repositories
{
    public interface ICommonCodeRepository : IRepositoryBase<CommonCode>
    {
        Task<CommonCodeDto[]> GetCommonCodesAsync();
        List<CommonCodeDto> GetCommonCodes();
        Task<CommonCodeDto> GetCode(string CodeSet, string CodeValue);
    }
    public class CommonCodeRepository : RepositoryBase<CommonCode>, ICommonCodeRepository
    {
        public CommonCodeRepository(AppDbContext dbContext, IMapper mapper, ICurrentUser currentUser)
            : base(dbContext, mapper, currentUser)
        {
        }

        public async Task<CommonCodeDto[]> GetCommonCodesAsync()
        {
            var commonCodes = await DbSet.AsNoTracking().ToArrayAsync();

            return Mapper.Map<CommonCodeDto[]>(commonCodes);
        }

        public List<CommonCodeDto> GetCommonCodes()
        {
            var commonCodes = DbSet.AsNoTracking().ToList();

            return Mapper.Map<List<CommonCodeDto>>(commonCodes);
        }

        public async Task<CommonCodeDto> GetCode(string codeSet, string codeValue)
        {
            var entity = await DbSet.AsNoTracking().FirstAsync(x => x.CodeSet == codeSet && x.CodeValue == codeValue);
            return Mapper.Map<CommonCodeDto>(entity);
        } 
    }
}