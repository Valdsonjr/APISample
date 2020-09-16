using Domain.Tipos;
using Domain.Validadores;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace V0.Api.Controllers
{
    /// <summary>
    /// Requisições de gerenciamento de itens
    /// </summary>
    [ApiController]
    [ApiVersion("0.1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly ItemService _service;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="service"></param>
        public ItemController(ItemService service)
        {
            _service = service;
        }

        /// <summary>
        /// Obtém um item pela chave
        /// </summary>
        /// <param name="key">chave de identificação do item</param>
        /// <response code="200">Item com a chave especificada</response>
        /// <response code="404">Item não encontrado</response>
        [HttpGet("{key}")]
        [ProducesResponseType(typeof(Item), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public ActionResult<Item> Get(string key)
        {
            var result = _service.Obter(key);
            return result != null ? (ActionResult<Item>) Ok(result) : NotFound();
        }

        /// <summary>
        /// Obtém todos os itens cadastrados
        /// </summary>
        /// <response code="200">Todos os itens cadastrados</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Item>), StatusCodes.Status200OK)]
        public IEnumerable<Item> Get() => _service.Obter();

        /// <summary>
        /// Insere um novo item
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">Item cadastrado</response>
        /// <response code="400">Lista de erros de validação</response>
        [HttpPost]
        [ProducesResponseType(typeof(Item), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<CreatedAtActionResult> Post([CustomizeValidator(RuleSet = "Common,Post")]Item item)
        {
            await _service.Inserir(item);
            var result = new CreatedAtActionResult(nameof(Get), "Item", new { version = "0.1", key = item.Key }, item);
            return result;
        }

        /// <summary>
        /// Remove um item
        /// </summary>
        /// <param name="key">chave do item para ser removido</param>
        /// <response code="204">Item removido com sucesso</response>
        /// <response code="404">Item não encontrado</response>
        [HttpDelete("{key}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(string key) 
            => await _service.Remover(key) ? (ActionResult) NoContent() 
                                           : NotFound();

        /// <summary>
        /// Atualiza um item
        /// </summary>
        /// <param name="key">chave do item para ser alterado</param>
        /// <param name="patches">alterações</param>
        /// <param name="validator">validador de itens</param>
        /// <response code="200">Item alterado com sucesso</response>
        /// <response code="400">Erros de validação</response>
        /// <response code="404">Item não encontrado</response>
        [HttpPatch("{key}")]
        [ProducesResponseType(typeof(Item), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Patch(string key, 
            [FromBody] JsonPatchDocument<Item> patches, 
            [FromServices] ItemValidator validator)
        {
            var item = _service.Obter(key);

            if (item == null)
                return NotFound();

            patches.ApplyTo(item);

            var validation = validator.Validate(item, ruleSet: "Common");

            if (!validation.IsValid)
                return BadRequest(validation);

            await _service.Alterar(item);

            return Ok(item);
        }
    }
}
