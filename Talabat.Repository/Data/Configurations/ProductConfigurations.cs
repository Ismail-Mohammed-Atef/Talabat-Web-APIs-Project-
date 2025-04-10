﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entites.Configurations
{
	internal class ProductConfigurations : IEntityTypeConfiguration<Product>
	{
		public void Configure(EntityTypeBuilder<Product> builder)
		{
			builder.Property(p => p.Name).IsRequired().HasMaxLength(108);
			builder.Property(p => p.Description).IsRequired();
			builder.Property(p => p.PictureUrl).IsRequired();
			builder.Property(p => p.Price).HasColumnType("decimal(18,2)");
			builder.HasOne(p => p.ProductBrand).WithMany().HasForeignKey(p => p.BrandId).OnDelete(DeleteBehavior.ClientSetNull);
			builder.HasOne(p => p.ProductType).WithMany().HasForeignKey(p => p.TypeId).OnDelete(DeleteBehavior.ClientSetNull);
		}
	}
}
