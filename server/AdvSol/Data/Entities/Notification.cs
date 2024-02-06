namespace AdvSol.Data.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Entity { get; set; }
        public int EntityId { get; set; }
        public bool IsRead { get; set; }
        public int UserId { get; set; }
        public virtual SystemUser User { get; set; }
    }
}
