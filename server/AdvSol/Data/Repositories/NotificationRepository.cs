using AdvSol.Data.Entities;
using AdvSol.Services.Dtos;
using AdvSol.Services.Dtos.Notification;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AdvSol.Data.Repositories
{
    public interface INotificationRepository : IRepositoryBase<Notification>
    {
        Task<NotificationDto[]> GetNotificationsAsync();
        Task UpdateNotificationAsync(int[] ids);
        Task CreateNotificationAsync(Notification notification);
    }
    public class NotificationRepository : RepositoryBase<Notification>, INotificationRepository
    {
        public NotificationRepository(AppDbContext dbContext, IMapper mapper, ICurrentUser currentUser)
            : base(dbContext, mapper, currentUser)
        {
        }

        public async Task<NotificationDto[]> GetNotificationsAsync()
        {
            var notifications = await DbSet.AsNoTracking()
                .Include(x => x.User)
                .Where(x => x.UserId == _currentUser.Id && x.IsRead == false)
                .ToArrayAsync();

            return Mapper.Map<NotificationDto[]>(notifications);
        }

        public async Task CreateNotificationAsync(Notification notification)
        {
            await DbContext.AddAsync(notification);
        }

        public async Task UpdateNotificationAsync(int[] ids)
        {
            var notifications = await DbSet
                .Include(x => x.User)
                .Where(x => x.UserId == _currentUser.Id && x.IsRead == false && ids.Contains(x.Id))
                .ToArrayAsync();

            foreach (var notification in notifications)
            {
                notification.IsRead = true;
            }
        }

    }
}