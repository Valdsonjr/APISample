using Data.EFCore.Contextos;
using Domain.Tipos;
using Domain.Repositorios;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Domain.UnitOfWork;

namespace Data.EFCore.Repositorios
{
    public class EFCoreItemRepository : IItemRepository
    {
        private readonly ItemContext context;
        public EFCoreItemRepository(ItemContext context)
        {
            this.context = context;
        }

        bool IItemRepository.Alterar(Item item)
        { 
            context.Entry(item).State = EntityState.Modified;
            return true;
        } 

        void IUnitOfWork.Commit()
            => context.SaveChanges();

        bool IItemRepository.Inserir(Item item)
        {
            var i = context.Items.Find(item.Key);
            if (i == null)
                context.Items.Add(item);

            return i == null;
        }

        Item? IItemRepository.ObterPorId(string key)
            => context.Find<Item>(key);

        IQueryable<Item> IItemRepository.ObterTodos()
            => context.Items;

        bool IItemRepository.Remover(string key)
        {
            var item = context.Find<Item>(key);
            if (item != null)
                context.Remove(item);

            return item != null;
        }
    }
}
