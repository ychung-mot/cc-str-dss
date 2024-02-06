using AdvSol.Data.Repositories;
using AdvSol.Data;
using AdvSol.Services.Dtos.SystemUser;
using AdvSol.Services.Dtos;
using AdvSol.Utils;
using AutoMapper;

namespace AdvSol.Services
{
    public interface ISystemUserService
    {
        Task<SystemUserDto[]> GetSystemUsersAsync();
        Task<SystemUserDto?> GetSystemUserAsync(int id);
        Task<SystemUserDto?> GetSystemUserByUsernameAsync(string username);
        Task<(ApplicantDto? dto, Dictionary<string, List<string>> errors)> CreateSystemUserAsync(SystemUserDto dto);
        Task<bool> CheckCredentials(CredentialDto dto);
    }
    public class SystemUserService : ServiceBase, ISystemUserService
    {
        private readonly ISystemUserRepository _systemUserRepo;
        private readonly ICommonCodeRepository _commonCodeRepo;
        private readonly IAddressService _addressService;

        public SystemUserService(ICurrentUser currentUser, IFieldValidatorService validator, 
            ISystemUserRepository systemUserRepo, ICommonCodeRepository commonCodeRepo, IAddressService addressService,
            IUnitOfWork unitOfWork, IMapper mapper)
            : base(currentUser, validator, unitOfWork, mapper)
        {
            _systemUserRepo = systemUserRepo;
            _commonCodeRepo = commonCodeRepo;
            _addressService = addressService;
            _unitOfWork = unitOfWork;
        }

        public async Task<SystemUserDto[]> GetSystemUsersAsync()
        {
            return await _systemUserRepo.GetSystemUsersAsync();
        }
        public async Task<SystemUserDto?> GetSystemUserAsync(int id)
        {
            return await _systemUserRepo.GetSystemUserAsync(id);
        }

        public async Task<SystemUserDto?> GetSystemUserByUsernameAsync(string username)
        {
            return await _systemUserRepo.GetSystemUserByUsernameAsync(username);
        }

        public async Task<(ApplicantDto? dto, Dictionary<string, List<string>> errors)> CreateSystemUserAsync(SystemUserDto dto)
        {
            var errors = new Dictionary<string, List<string>>();

            errors = _validator.Validate(Entities.SystemUser, dto, errors);

            await ValidateSystemUser(dto, errors);

            if (errors.Count > 0)
            {
                return (null, errors);
            }

            dto.Password = dto.Password.ComputeSHA256Hash();

            var role = await _commonCodeRepo.GetCode("ROLE", Roles.Applicant);
            dto.RoleId = role.Id;

            dto.DateCreated = DateTime.UtcNow;

            var entity = await _systemUserRepo.CreateSystemUserAsync(dto);

            _unitOfWork.Commit();

            return (_mapper.Map<ApplicantDto>(entity), errors);
        }

        private async Task<Dictionary<string, List<string>>> ValidateSystemUser(SystemUserDto dto, Dictionary<string, List<string>> errors)
        {
            //maximum number of applicants
            var count = await _systemUserRepo.GetNumberOfApplicantsAsync();
            if (count >= 20)
            {
                errors.AddItem("Entity", "Maximum number of applicants reached. Cannot create applicant anymore.");
            }

            //Validate duplicate user
            var existingUser = await _systemUserRepo.GetSystemUserByUsernameOrLastNameAsync(dto.Username ?? "", dto.LastName ?? "");

            if (existingUser != null)
            {
                errors.AddItem("Username", "Username or Lastname already exists.");
            }

            //Validate Address and Postal Code
            await _addressService.ValidateAddress(dto, errors);

            return errors;
        }

        public async Task<bool> CheckCredentials(CredentialDto dto)
        {
            return await _systemUserRepo.CheckCredentials(dto);
        }
    }
}
