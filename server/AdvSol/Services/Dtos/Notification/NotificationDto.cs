using AdvSol.Services.Dtos.SystemUser;
using System.Text.Json.Serialization;

namespace AdvSol.Services.Dtos.Notification
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Entity { get; set; }
        public int EntityId { get; set; }
        public bool IsRead { get; set; }
        public int UserId { get; set; }
        [JsonIgnore]
        public virtual SystemUserDto User { get; set; }
    }
}
