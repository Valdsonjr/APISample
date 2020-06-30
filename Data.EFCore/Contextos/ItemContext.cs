using Domain.Tipos;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Data.EFCore.Contextos
{
    public class ItemContext : DbContext
    {
        public DbSet<Item> Items { get; set; }

#pragma warning disable CS8618 // O campo não anulável não foi inicializado. Considere declará-lo como anulável.
        public ItemContext(DbContextOptions<ItemContext> options)
#pragma warning restore CS8618 // O campo não anulável não foi inicializado. Considere declará-lo como anulável.
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
