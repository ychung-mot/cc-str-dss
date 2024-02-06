using AdvSol.Data.Repositories;
using AdvSol.Data;
using AdvSol.Services.Dtos.Notification;
using AdvSol.Services.Dtos;
using AutoMapper;

namespace AdvSol.Services
{
    public interface INotificationService
    {
        Task<NotificationDto[]> GetNotificationsAsync();
        Task UpdateNotificationAsync(int[] ids);
    }
    public class NotificationService : ServiceBase, INotificationService
    {
        private readonly INotificationRepository _notificationRepo;

        public NotificationService(ICurrentUser currentUser, IFieldValidatorService validator, INotificationRepository notificationRepo, IUnitOfWork unitOfWork, IMapper mapper)
            : base(currentUser, validator, unitOfWork, mapper)
        {
            _notificationRepo = notificationRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<NotificationDto[]> GetNotificationsAsync()
        {
            return await _notificationRepo.GetNotificationsAsync();
        } 

        public async Task UpdateNotificationAsync(int[] ids)
        {
            if (ids == null || ids.Length == 0)
                return;

            await _notificationRepo.UpdateNotificationAsync(ids);

            _unitOfWork.Commit();
        }
    }
}
