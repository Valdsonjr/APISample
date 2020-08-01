using Domain.Repositorios;
using Domain.Tipos;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    public class ItemService
    {
        private readonly IItemRepository itemRepository;

        public ItemService(IItemRepository itemRepository)
        {
            this.itemRepository = itemRepository;
        }

        public Item? ObterPorId(String key) => itemRepository.Obter(key);
        public IQueryable<Item> ObterTodos() => itemRepository.Obter();
        public async Task Inserir(Item item)
        {
            itemRepository.Inserir(item);
            await itemRepository.Commit();
        }
        public async Task<bool> Remover(String key)
        {
            var res = itemRepository.Remover(key);
            await itemRepository.Commit();
            return res;
        }
        public async Task<bool> Alterar(Item item)
        { 
            var res = itemRepository.Alterar(item);
            await itemRepository.Commit();
            return res;
        } 
    }
}
