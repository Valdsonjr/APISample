using Domain.Repositories;
using Domain.Services;
using Domain.Types;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;

        public ItemService(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        IQueryable<Item> IItemService.Obter()
            => _itemRepository.Obter();

        Task IItemService.Inserir(Item item)
        {
            _itemRepository.Inserir(item);
            return _itemRepository.Commit();
        }

        Task IItemService.Remover(string key)
        {
            _itemRepository.Remover(key);
            return _itemRepository.Commit();
        }
    }
}
