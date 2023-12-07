using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Diagnostics;

namespace Template.WebApp.HealthChecks
{
    public class LatencyHealthCheck : IHealthCheck
    {
        public LatencyHealthCheck()
        {
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var stopwatch = Stopwatch.StartNew();
            var _urlToCheck = "Obtener de configuración";

            try
            {
                using (var httpClient = new HttpClient())
                {
                    // Simula una solicitud a un recurso externo, como una API
                    var response = await httpClient.GetAsync(_urlToCheck, cancellationToken);

                    // Ajusta la lógica según sea necesario según la respuesta real del servicio
                    if (response.IsSuccessStatusCode)
                    {
                        stopwatch.Stop();
                        var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

                        if (elapsedMilliseconds < 100)
                        {
                            return HealthCheckResult.Healthy($"Service Ok - Latency: {elapsedMilliseconds}ms");
                        }
                        else if (elapsedMilliseconds < 500)
                        {
                            return HealthCheckResult.Degraded($"Service slow - Latency: {elapsedMilliseconds}ms");
                        }
                        else
                        {
                            return HealthCheckResult.Unhealthy($"Service down - Latency: {elapsedMilliseconds}ms");
                        }
                    }
                    else
                    {
                        return HealthCheckResult.Unhealthy($"Service returned an error: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy($"Error checking service health: {ex.Message}");
            }
        }
    }
}
