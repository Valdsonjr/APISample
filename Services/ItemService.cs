using Domain.Repositorios;
using Domain.Tipos;
using System;
using System.Linq;

namespace Services
{
    public class ItemService
    {
        private readonly IItemRepository itemRepository;

        public ItemService(IItemRepository itemRepository)
        {
            this.itemRepository = itemRepository;
        }

        public Item? ObterPorId(String key) => itemRepository.ObterPorId(key);
        public IQueryable<Item> ObterTodos() => itemRepository.ObterTodos();
        public void Inserir(Item item)
        {
            itemRepository.Inserir(item);
            itemRepository.Commit();
        }
        public bool Remover(String key)
        {
            var res = itemRepository.Remover(key);
            itemRepository.Commit();
            return res;
        }
        public bool Alterar(Item item)
        { 
            var res = itemRepository.Alterar(item);
            itemRepository.Commit();
            return res;
        } 
    }
}
