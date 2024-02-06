using AdvSol.Data.Repositories;
using AdvSol.Data;
using AdvSol.Services.Dtos.CommonCode;
using AdvSol.Services.Dtos;
using AdvSol.Utils;
using AutoMapper;

namespace AdvSol.Services
{
    public interface ICommonCodeService
    {
        Task<CommonCodeDto[]> GetCommonCodesAsync();
    }
    public class CommonCodeService : ServiceBase, ICommonCodeService
    {
        private readonly ICommonCodeRepository _commonCodeRepo;

        public CommonCodeService(ICurrentUser currentUser, IFieldValidatorService validator, ICommonCodeRepository commonCodeRepo, IUnitOfWork unitOfWork, IMapper mapper)
            : base(currentUser, validator, unitOfWork, mapper)
        {
            _commonCodeRepo = commonCodeRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<CommonCodeDto[]> GetCommonCodesAsync()
        {
            return await _commonCodeRepo.GetCommonCodesAsync();
        } 
    }
}
