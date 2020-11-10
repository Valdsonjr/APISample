using Domain.Types;
using Domain.UnitOfWork;
using System;
using System.Linq;

namespace Domain.Repositories
{
    /// <summary>
    /// Repositório de itens
    /// 
    /// PS: como só existe um repositório não há problema de colocar ele como unidade de trabalho,
    /// mas caso você tenha mais de um repositório usando o mesmo contexto de banco é bom criar uma classe agregadora
    /// que implementa a interface IUnitOfWork
    /// </summary>
    public interface IItemRepository : IUnitOfWork
    {
        /// <summary>
        /// Obtém todos os itens cadastrados
        /// </summary>
        /// <returns></returns>
        IQueryable<Item> Obter();
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
    }
}
