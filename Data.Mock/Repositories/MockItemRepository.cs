using Domain.Repositories;
using Domain.Types;
using Domain.UnitOfWork;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Mock.Repositories
{
    public class MockItemRepository : IItemRepository
    {
        private readonly List<Item> _data;

        public MockItemRepository()
        {
            _data = new List<Item>
            {
                new Item { Key = "K1", Value = "V1" },
                new Item { Key = "K2", Value = "V2" },
                new Item { Key = "K3", Value = "V3" },
                new Item { Key = "K4", Value = "V4" },
                new Item { Key = "K5", Value = "V5" },
            };
        }

        public MockItemRepository(List<Item> data)
        {
            _data = data;
        }

        Task IUnitOfWork.Commit()
            => Task.CompletedTask;

        bool IItemRepository.Inserir(Item item)
            => !_data.Exists(i => i.Key == item.Key);

        IQueryable<Item> IItemRepository.Obter()
            => _data.AsQueryable();

        bool IItemRepository.Remover(string key)
            => _data.Exists(i => i.Key == key);
    }
}
