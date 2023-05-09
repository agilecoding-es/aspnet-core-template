using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Template.MvcWebApp.HealthChecks
{
    public class RedisHelthCheck : IHealthCheck
    {
        //private readonly IConnectionMultiplexer _redisConnection;

        //public RedisHelthCheck(IConnectionMultiplexer redisConnection)
        //{
        //    _redisConnection = redisConnection;
        //}

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            //    try
            //    {
            //        var database = _redisConnection.GetDatabase();

            //        database.StringGet("health");
            return Task.FromResult(HealthCheckResult.Healthy());
            //    }
            //    catch
            //    {
            //        return Task.FromResult(HealthCheckResult.Unhealthy());
            //    }
        }
    }
}
