using Infrasturcture.Data.Configurations;
using Infrasturcture.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Infrasturcture.Data
{
    public class GiftExchangerContext : IdentityDbContext<UserGE>
    {
        public GiftExchangerContext()
        { }
        public GiftExchangerContext(DbContextOptions options) : base(options)
        { }

        public DbSet<CreditTransfer> CreditTransfers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var configuration = new ConfigurationBuilder()
                                                             .SetBasePath(Directory.GetCurrentDirectory())
                                                             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                                             .Build();
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("LocalConnectionString"));
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserGE_CFG());
            builder.ApplyConfiguration(new CreditTransfer_CFG());
            base.OnModelCreating(builder);
        }
    }
}