namespace AdvSol.Services.Dtos
{
    public class EventEmail
    {
        public string EventDescription { get; set; } = "";
        public List<string> EmailAdresses { get; set; } = new List<string>();
    }
}
