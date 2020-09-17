using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Api.Infrastructure
{
    /// <summary>
    /// Realiza o monitoramento e log dos endpoints
    /// </summary>
    public class Monitoring : IActionFilter
    {
        private readonly Stopwatch stopwatch;
        private MonitoringResult result;
        private readonly ILogger<Monitoring> log;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="log">logger para monitoramento de endpoints</param>
        public Monitoring(ILogger<Monitoring> log)
        {
            this.log = log;
            result = new MonitoringResult();
            stopwatch = new Stopwatch();
        }

        /// <summary>
        /// Ao finalizar a execução
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            stopwatch.Stop();

            result.TimeElapsed = stopwatch.Elapsed;
            result.Result = context.Result.ToString();

            log.LogTrace(JsonConvert.SerializeObject(result, Formatting.Indented));

            stopwatch.Reset();
        }

        /// <summary>
        /// Antes de iniciar a execução
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            result = new MonitoringResult
            {
                Action = context.ActionDescriptor.DisplayName,
                Date = DateTime.UtcNow,
                Parameters = context.ActionArguments,
            };
            
            stopwatch.Start();
        }

        private class MonitoringResult
        {
            public string? Action { get; set; }
            public DateTime Date { get; set; }
            public IDictionary<string, object>? Parameters { get; set; }
            public string? Result { get; set; }
            public TimeSpan TimeElapsed { get; set; }
        }
    }
}
