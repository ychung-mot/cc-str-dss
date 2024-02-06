using Hangfire;

namespace AdvSol.Services.Jobs
{
    public class HelloWorldJob
    {
        [Queue("default")]
        [SkipSameJob]
        [AutomaticRetry(Attempts = 0)]
        public async Task ExecuteHelloWorld()
        {
            await Task.CompletedTask;
            Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm")} Hello World");
        }
    }
}
