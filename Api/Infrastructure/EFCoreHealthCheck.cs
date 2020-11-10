using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Infrastructure
{
    /// <summary>
    /// Health Check genérico para conexões com bancos de dados.
    /// </summary>
    public class EFCoreHealthCheck<T> : IHealthCheck
        where T : DbContext
    {
        private readonly DbContext _context;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="context"></param>
        public EFCoreHealthCheck(T context)
        {
            _context = context;
        } 

        async Task<HealthCheckResult> IHealthCheck.CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken)
        {
            try
            {
                var canConnect = await _context.Database.CanConnectAsync(cancellationToken);
                var data = new Dictionary<String, Object>
                {
                    { "ConnectionString", _context.Database.GetDbConnection().ConnectionString }
                };

                return canConnect
                    ? HealthCheckResult.Healthy(description: "Connection successful", data: data)
                    : HealthCheckResult.Unhealthy(description: "Connection unsucessful", data: data);
            }
            catch (Exception exception)
            {
                return HealthCheckResult.Unhealthy(description: "Connection unsucessful", exception);
            }
        }

    }
}
