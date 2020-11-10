using Domain.Types;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Services
{
    /// <summary>
    /// Interface de serviços de itens
    /// </summary>
    public interface IItemService
    {
        /// <summary>
        /// Obtém todos os itens
        /// </summary>
        /// <returns></returns>
        IQueryable<Item> Obter();

        /// <summary>
        /// Tenta inserir um novo item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task Inserir(Item item);
        /// <summary>
        /// Tenta remover um item
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task Remover(String key);
    }
}
