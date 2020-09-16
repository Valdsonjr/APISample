using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Diagnostics;

namespace Api.Infrastructure
{
    /// <summary>
    /// Realiza o monitoramento e log dos endpoints
    /// </summary>
    public class Monitoring : IActionFilter
    {
        private readonly Stopwatch stopwatch = new Stopwatch();

        /// <summary>
        /// Ao finalizar a execução
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            stopwatch.Stop();

            Console.WriteLine("Resultado: {0}", context.Result);
            Console.WriteLine("Duração: {0}", stopwatch.Elapsed);

            stopwatch.Reset();
        }

        /// <summary>
        /// Antes de iniciar a execução
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine("-------------------------------------------------------------------------------------");
            Console.WriteLine("Ação: {0}", context.ActionDescriptor.DisplayName);
            Console.WriteLine("Horário: {0}", DateTime.Now);
            Console.WriteLine("Parâmetros:");
            Console.WriteLine(JsonConvert.SerializeObject(context.ActionArguments, Formatting.Indented));
            
            stopwatch.Start();
        }
    }
}
