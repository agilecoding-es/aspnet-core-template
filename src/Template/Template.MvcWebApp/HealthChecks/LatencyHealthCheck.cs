using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Template.MvcWebApp.HealthChecks
{
    public class LatencyHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            int latency = 0;

            var rnd = new Random();

            latency = rnd.Next(1, 15);

            if (latency < 5)
            {
                return Task.FromResult(HealthCheckResult.Healthy($"Service Ok - {latency}"));
            }
            else if (latency < 10)
            {
                return Task.FromResult(HealthCheckResult.Degraded($"Service slow - {latency}"));
            }
            else
            {
                return Task.FromResult(HealthCheckResult.Unhealthy($"Service down- {latency}"));
            }
        }
    }
}
