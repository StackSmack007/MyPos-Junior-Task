﻿using Infrasturcture.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrasturcture.Data.Configurations
{
    public class UserGE_CFG : IEntityTypeConfiguration<UserGE>
    {
        public void Configure(EntityTypeBuilder<UserGE> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasMaxLength(128);
            builder.Property(x => x.PhoneNumber).IsRequired();
        }
    }
}
