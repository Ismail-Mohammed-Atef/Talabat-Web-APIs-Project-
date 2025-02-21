using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models.Orders;
using Order = Talabat.Core.Models.Orders.Order;

namespace Talabat.Repository.Data.Configuration 
{
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(o => o.SubTotal).HasColumnType("decimal(18,2)");
            builder.Property(o => o.Status).HasConversion(Ostatus => Ostatus.ToString(), ostatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), ostatus));
            builder.OwnsOne(o => o.ShippingAddress, SA => SA.WithOwner());
            builder.HasOne(o=>o.DeliveryMethod).WithMany().OnDelete(DeleteBehavior.NoAction);

        }
    }
}
