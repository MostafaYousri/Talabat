using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Repository.Data.Configurtions
{
    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(O => O.ShippingAddress, shippingAddress => shippingAddress.WithOwner()); // 1:1 total  من الطرفين

            builder.Property(O => O.Status)
                .HasConversion(
                OStatus => OStatus.ToString(),
                OStatus =>(OrderStatus) Enum.Parse(typeof(OrderStatus),OStatus)
                );

            //builder.HasOne(O => O.DeliveryMethod)
            //    .WithOne();

            //builder.HasIndex(O => O.DeliveryMethodId).IsUnique();

            builder.Property(O => O.Subtotal)
                .HasColumnType("decimal(18, 2)");

            builder.HasOne(o => o.DeliveryMethod)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
