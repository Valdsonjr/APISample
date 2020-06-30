using Domain.Tipos;
using Domain.Validadores;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EFCore
{
    public class ItemMapping : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.HasKey(i => i.Key);

            builder.Property(i => i.Key)
                   .IsRequired()
                   .IsUnicode()
                   .HasMaxLength(ItemValidator.KeyMaxLength);

            builder.Property(i => i.CreationDate)
                   .IsRequired();

            builder.Property(i => i.Value)
                   .IsRequired()
                   .IsUnicode(true)
                   .HasMaxLength(ItemValidator.ValueMaxLength);
        }
    }
}
