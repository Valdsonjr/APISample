using Api.v0.Models;
using Api.v0.Queries;
using AutoMapper;
using Domain.Services;
using Domain.Types;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.v0.Controllers
{
    /// <summary>
    /// Requisições de gerenciamento de itens
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _service;
        private readonly IMapper _mapper;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="service"></param>
        /// <param name="mapper"></param>
        public ItemController(IItemService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtém todos os itens cadastrados
        /// </summary>
        /// <response code="200">Todos os itens cadastrados</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ItemTO>), StatusCodes.Status200OK)]
        public IEnumerable<ItemTO> Get([FromQuery] ItemQuery query)
            => _mapper.ProjectTo<ItemTO>(_service.Obter())
                      .Where(i => query.Key == null || i.Key == query.Key)
                      .Where(i => query.CreationDateInit == null || i.CreationDate >= query.CreationDateInit)
                      .Where(i => query.CreationDateEnd == null || i.CreationDate <= query.CreationDateEnd)
                      ;

        /// <summary>
        /// Insere um novo item
        /// </summary>
        /// <param name="itemTO"></param>
        /// <response code="201">Item cadastrado</response>
        /// <response code="400">Lista de erros de validação</response>
        [HttpPost]
        [ProducesResponseType(typeof(ItemTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<CreatedAtActionResult> Post([CustomizeValidator(RuleSet = "default,Post")]ItemTO itemTO)
        {
            await _service.Inserir(_mapper.Map<Item>(itemTO));

            var result = new CreatedAtActionResult(nameof(Get), "Item", new 
            { 
                key = itemTO.Key 
            }, itemTO);

            return result;
        }

        /// <summary>
        /// Remove um item
        /// </summary>
        /// <param name="key">chave do item para ser removido</param>
        /// <response code="204">Item removido com sucesso</response>
        [HttpDelete("{key}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public async Task<NoContentResult> Delete(string key)
        {
            await _service.Remover(key);
            return NoContent();
        }
    }
}
