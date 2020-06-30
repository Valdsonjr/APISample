using Domain.Tipos;
using System;
using System.Linq;

namespace Domain.Repositorios
{
    /// <summary>
    /// Repositório de itens
    /// </summary>
    public interface IItemRepository : IRepository
    {
        /// <summary>
        /// Obtém todos os itens cadastrados
        /// </summary>
        /// <returns></returns>
        IQueryable<Item> ObterTodos();
        /// <summary>
        /// Tenta obter um item através de sua chave
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Item? ObterPorId(String key);
        /// <summary>
        /// Tenta inserir um item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Boolean Inserir(Item item);
        /// <summary>
        /// Tenta deletar um item
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Boolean Remover(String key);
        /// <summary>
        /// Tenta alterar um item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Boolean Alterar(Item item);
    }
}
