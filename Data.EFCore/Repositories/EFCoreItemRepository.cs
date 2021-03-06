﻿using Data.EFCore.Contexts;
using Domain.Repositories;
using Domain.Types;
using Domain.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Data.EFCore.Repositories
{
    public class EFCoreItemRepository : IItemRepository
    {
        private readonly ItemContext _context;
        public EFCoreItemRepository(ItemContext context)
        {
            _context = context;
        }

        Task IUnitOfWork.Commit()
            => _context.SaveChangesAsync();

        Boolean IItemRepository.Inserir(Item item)
        {
            var i = _context.Items.Find(item.Key);
            if (i == null)
                _context.Items.Add(item);

            return i == null;
        }

        IQueryable<Item> IItemRepository.Obter()
            => _context.Items.AsNoTracking();

        Boolean IItemRepository.Remover(String key)
        {
            var item = _context.Find<Item>(key);
            if (item != null)
                _context.Remove(item);

            return item != null;
        }
    }
}
