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
        private readonly Stopwatch _stopwatch;
        private MonitoringResult _result;
        private readonly ILogger<Monitoring> _log;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="log">logger para monitoramento de endpoints</param>
        public Monitoring(ILogger<Monitoring> log)
        {
            _log = log;
            _result = new MonitoringResult();
            _stopwatch = new Stopwatch();
        }

        /// <summary>
        /// Ao finalizar a execução
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _stopwatch.Stop();

            _result.TimeElapsed = _stopwatch.Elapsed;
            _result.Result = context?.Result?.ToString() ?? "exception";

            _log.LogTrace(JsonConvert.SerializeObject(_result, Formatting.Indented));

            _stopwatch.Reset();
        }

        /// <summary>
        /// Antes de iniciar a execução
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            _result = new MonitoringResult
            {
                Action = context.ActionDescriptor.DisplayName,
                Date = DateTime.UtcNow,
                Parameters = context.ActionArguments,
            };
            
            _stopwatch.Start();
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
