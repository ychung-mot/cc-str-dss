using Hangfire.Client;
using Hangfire.Common;
using Hangfire.Server;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdvSol.Services.Jobs
{
    public sealed class SkipSameJobAttribute : JobFilterAttribute, IClientFilter, IServerFilter
    {
        private readonly int _timeoutInSeconds = 1;

        public void OnCreated(CreatedContext filterContext)
        {
        }

        public void OnCreating(CreatingContext context)
        {
            try
            {
                var job = context.Job;
                var jobFingerprint = GetJobFingerprint(job);

                var monitor = context.Storage.GetMonitoringApi();
                var fingerprints = monitor.ProcessingJobs(0, int.MaxValue)
                    .Select(x => GetJobFingerprint(x.Value.Job))
                    .ToList();

                fingerprints.AddRange(
                    monitor.EnqueuedJobs("default", 0, int.MaxValue)
                    .Select(x => GetJobFingerprint(x.Value.Job))
                );

                foreach (var fingerprint in fingerprints)
                {
                    if (jobFingerprint != fingerprint)
                        continue;

                    Console.WriteLine($"{fingerprint} cancelled");
                    context.Canceled = true;
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking for existing jobs: {ex}");
            }
        }

        public void OnPerforming(PerformingContext filterContext)
        {
            try
            {
                var resource = GetJobFingerprint(filterContext.BackgroundJob.Job);
                var timeout = TimeSpan.FromSeconds(_timeoutInSeconds);

                var distributedLock = filterContext.Connection.AcquireDistributedLock(resource, timeout);
                filterContext.Items["DistributedLock"] = distributedLock;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error acquiring distributed lock: {ex}");
            }
        }

        public void OnPerformed(PerformedContext filterContext)
        {
            try
            {
                if (!filterContext.Items.ContainsKey("DistributedLock"))
                {
                    throw new InvalidOperationException("Cannot release a distributed lock: it was not acquired.");
                }

                var distributedLock = (IDisposable)filterContext.Items["DistributedLock"];
                distributedLock.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error releasing distributed lock: {ex}");
            }
        }

        private string GetJobFingerprint(Job job)
        {
            var args = "";

            if (job.Args.Count > 0)
            {
                args = "-" + JsonConvert.SerializeObject(job.Args);
            }

            return $"{job.Type.FullName}-{job.Method.Name}{args}";
        }
    }
}
