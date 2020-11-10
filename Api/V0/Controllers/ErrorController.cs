using Domain.Resources;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Api.v0.Controllers
{
    /// <summary>
    /// Controlador de erros da aplicação
    /// </summary>
    [ApiController]
    [ApiVersion("1")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ErrorController : ControllerBase
    {
        private readonly IStringLocalizer<ErrorMessages> _localizer;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="localizer"></param>
        public ErrorController(IStringLocalizer<ErrorMessages> localizer)
        {
            _localizer = localizer;
        }

        /// <summary>
        /// Endpoint de Erros de produção
        /// </summary>
        /// <returns></returns>
        [Route("Production")]
        public IActionResult Error()
            => Problem(title: _localizer["GlobalError"]);

        /// <summary>
        /// Endpoint de Erros de desenvolvimento
        /// </summary>
        /// <returns></returns>
        [Route("Development")]
        public IActionResult ErrorDev()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context?.Error;

            return Problem(title: exception?.Message, 
                           instance: exception?.StackTrace);
        }
    }
}
