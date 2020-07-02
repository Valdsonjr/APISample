using Domain.Repositorios;
using Domain.Tipos;
using Domain.UnitOfWork;
using System.Collections.Generic;
using System.Linq;

namespace Data.Mock.Repositorios
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

        void IUnitOfWork.Commit()
        {
            return;
        }

        bool IItemRepository.Inserir(Item item)
            => !_data.Exists(i => i.Key == item.Key);

        Item? IItemRepository.Obter(string key)
            => _data.Find(i => i.Key == key);

        IQueryable<Item> IItemRepository.Obter()
            => _data.AsQueryable();

        bool IItemRepository.Remover(string key)
            => _data.Exists(i => i.Key == key);

        bool IItemRepository.Alterar(Item item)
            => _data.Exists(i => i.Key == item.Key);
    }
}
