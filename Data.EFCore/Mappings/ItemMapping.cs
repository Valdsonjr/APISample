using Domain.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EFCore.Mappings
{
    public class ItemMapping : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.HasKey(i => i.Key);

            builder.Property(i => i.Key)
                   .IsRequired()
                   .IsUnicode()
                   .HasMaxLength(Item.KeyMaxLength);

            builder.Property(i => i.CreationDate)
                   .IsRequired();

            builder.Property(i => i.Value)
                   .IsRequired()
                   .IsUnicode(true)
                   .HasMaxLength(Item.ValueMaxLength);
        }
    }
}
