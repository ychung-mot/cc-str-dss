namespace AdvSol.Data.Entities
{
    public class Audit
    {
        public int Id { get; set; }
        public string Entity { get; set; }
        public int EntityId { get; set; }
        public string Field { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string Operation { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
}
