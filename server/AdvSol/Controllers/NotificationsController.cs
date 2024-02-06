using AdvSol.Authorization;
using AdvSol.Services;
using AdvSol.Services.Dtos;
using AdvSol.Services.Dtos.Notification;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AdvSol.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : BaseApiController
    {
        private INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService, ICurrentUser currentUser, IMapper mapper)
            : base(currentUser, mapper)
        {
            _notificationService = notificationService;
        }

        [AdvAuthorize()]
        [HttpGet("", Name = "GetNotifications")]
        public async Task<ActionResult<NotificationDto[]>> GetNotificationsAsync()
        {
            var notifications = await _notificationService.GetNotificationsAsync();

            return Ok(notifications);
        }

        [AdvAuthorize()]
        [HttpPut("", Name = "UpdateNotifications")]
        public async Task<ActionResult<NotificationDto[]>> UpdateNotificationsAsync([FromQuery] int[] ids)
        {
            await _notificationService.UpdateNotificationAsync(ids);

            return Ok();
        }

    }
}
