using AdvSol.Data.Repositories;
using AdvSol.Data;
using AdvSol.Services.Dtos.StrApplication;
using AdvSol.Services.Dtos;
using AdvSol.Utils;
using AutoMapper;
using AdvSol.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace AdvSol.Services
{
    public interface IStrApplicationService
    {
        Task<StrApplicationDto[]> GetStrApplicationsAsync();
        Task<StrApplicationDto[]> GetStrApplicationsByApplicantAsync(int applicantId);
        Task<StrApplicationDto?> GetStrApplicationAsync(int id);
        Task<(StrApplicationDto? dto, Dictionary<string, List<string>> errors)> CreateStrApplicationAsync(StrApplicationDto dto);
        Task<(bool notFound, Dictionary<string, List<string>> errors)> UpdateStrApplicationAsync(StrApplicationDto dto);
    }
    public class StrApplicationService : ServiceBase, IStrApplicationService
    {
        private readonly IStrApplicationRepository _strApplicationRepo;
        private IAddressService _addressService;
        private ICommonCodeRepository _commonCodeRepo;
        private INotificationRepository _notificationRepo;
        private ISystemUserRepository _systemUserRepo;
        private IAuditRepository _auditRepo;

        public StrApplicationService(ICurrentUser currentUser, IFieldValidatorService validator, IStrApplicationRepository strApplicationRepo, 
            IAddressService addressService, ICommonCodeRepository commonCodeRepo, INotificationRepository notificationRepo, ISystemUserRepository systemUserRepo,
            IAuditRepository auditRepo,
            IUnitOfWork unitOfWork, IMapper mapper)
            : base(currentUser, validator, unitOfWork, mapper)
        {
            _strApplicationRepo = strApplicationRepo;
            _unitOfWork = unitOfWork;
            _addressService = addressService;
            _commonCodeRepo = commonCodeRepo;
            _notificationRepo = notificationRepo;
            _systemUserRepo = systemUserRepo;
            _auditRepo = auditRepo;
        }

        public async Task<StrApplicationDto[]> GetStrApplicationsAsync()
        {
            return await _strApplicationRepo.GetStrApplicationsAsync();
        }
        public async Task<StrApplicationDto?> GetStrApplicationAsync(int id)
        {
            return await _strApplicationRepo.GetStrApplicationAsync(id);
        }

        public async Task<(StrApplicationDto? dto, Dictionary<string, List<string>> errors)> CreateStrApplicationAsync(StrApplicationDto dto)
        {
            var errors = new Dictionary<string, List<string>>();

            var pendingStatus = await _commonCodeRepo.GetCode(CodeSet.ComplianceStatus, "Pending");

            dto.ComplianceStatusId = pendingStatus.Id;
            dto.ApplicantId = _currentUser.Id;
            dto.DateCreated = DateTime.UtcNow;

            if (dto.Province.IsEmpty()) dto.Province = "BC";

            errors = _validator.Validate(Entities.StrApplication, dto, errors);

            await ValidateStrApplication(dto, errors);

            if (errors.Count > 0)
            {
                return (null, errors);
            }

            var entity = await _strApplicationRepo.CreateStrApplicationAsync(dto);

            var adminUsers = await _systemUserRepo.GetAdminUsers();

            _unitOfWork.Commit();

            foreach (var user in adminUsers)
            {
                var notification = new Notification
                {
                    Title = $"A new application ({entity.StreetAddress}) has been created and needs to be reviewed for compliance",
                    Entity = "StrApplication",
                    EntityId = entity.Id,
                    IsRead = false,
                    UserId = user.Id,
                };

                await _notificationRepo.CreateNotificationAsync(notification);
            }

            _auditRepo.CreateAudits(entity);

            _unitOfWork.Commit();

            dto = await _strApplicationRepo.GetStrApplicationAsync(entity.Id);

            return (dto, errors);
        }

        private async Task<Dictionary<string, List<string>>> ValidateStrApplication(StrApplicationDto dto, Dictionary<string, List<string>> errors)
        {
            var count = await _strApplicationRepo.GetNumberOfStrApplicationsByApplicantAsync(dto.ApplicantId);
            if (count > 5)
            {
                errors.AddItem("Count", "Already reached the maximum number (5) of permits allowed for a given applicant.");
            }

            //see if there are any application tha has the same lat/long

            await _addressService.ValidateAddress(dto, errors);

            return errors;
        }

        public async Task<StrApplicationDto[]> GetStrApplicationsByApplicantAsync(int applicantId)
        {
            return await _strApplicationRepo.GetStrApplicationsByApplicantAsync(applicantId);
        }

        public async Task<(bool notFound, Dictionary<string, List<string>> errors)> UpdateStrApplicationAsync(StrApplicationDto dto)
        {
            var entity = await _strApplicationRepo.GetStrApplicationAsync(dto.Id);

            if (entity == null) return (true, null);

            dto.DateCreated = entity.DateCreated;
            dto.Longitude = entity.Longitude;
            dto.Latitude = entity.Latitude;

            var errors = new Dictionary<string, List<string>>();

            if (_currentUser.Role != Roles.Admin)
            {
                if (dto.ComplianceStatusId != entity.ComplianceStatusId)
                {
                    errors.AddItem("Authorization", "Unable to modify compliance status. Insufficient permissions.");
                    return (false, errors);
                }

                if (dto.ApplicantId != entity.ApplicantId || dto.ApplicantId != _currentUser.Id)
                {
                    errors.AddItem("Authorization", "Unable to modify another applicant's application. Insufficient privileges.");
                    return (false, errors);
                }
            }

            var isStatusChanged = dto.ComplianceStatusId != entity.ComplianceStatusId;

            errors = _validator.Validate(Entities.StrApplication, dto, errors);

            await ValidateStrApplication(dto, errors);

            if (errors.Count > 0)
            {
                return (false, errors);
            }

            await _strApplicationRepo.UpdateStrApplicationAsync(dto);

            if (isStatusChanged)
            {
                var notification = new Notification
                {
                    Title = $"Your application ({entity.StreetAddress}) status has been changed.",
                    Entity = "StrApplication",
                    EntityId = entity.Id,
                    IsRead = false,
                    UserId = entity.ApplicantId,
                };

                await _notificationRepo.CreateNotificationAsync(notification);
            }

            _unitOfWork.Commit();

            return (false, errors);
        }
    }
}
