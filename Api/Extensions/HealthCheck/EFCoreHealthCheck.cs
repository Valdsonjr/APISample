using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Extensions.HealthCheck
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
            var data = new Dictionary<String, Object>();
            try
            {
                var canConnect = await _context.Database.CanConnectAsync(cancellationToken);
                data.Add("ConnectionString", _context.Database.GetDbConnection().ConnectionString);

                return canConnect
                    ? HealthCheckResult.Healthy(description: "Connection successful", data: data)
                    : HealthCheckResult.Unhealthy(description: "Connection unsucessful", data: data);
            }
            catch (Exception exception)
            {
                // HealthCheckResult.Unhealthy pode receber a exceção como parâmetro
                // porém o System.Text.JSON ainda não trata ciclos de referências (que existem no objeto da exceção)
                // portanto nós usamos o stacktrace somente
                data.Add("stack_trace", exception.StackTrace ?? exception.Message);
                return HealthCheckResult.Unhealthy(description: "Connection unsucessful", data: data);
            }
        }

    }
}
