using Infrasturcture.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrasturcture.Data.Configurations
{
    public class CreditTransfer_CFG : IEntityTypeConfiguration<CreditTransfer>
    {
        public void Configure(EntityTypeBuilder<CreditTransfer> builder)
        {
            builder.HasKey(ct => ct.Id);
            builder.Property(ct => ct.Comment).HasMaxLength(1024).IsRequired(false);
            builder.HasOne(ct => ct.Reciever).WithMany(u => u.TransactionsRecieved).HasForeignKey(ct => ct.RecieverId);
            builder.HasOne(ct => ct.Sender).WithMany(u => u.TransactionsSent).HasForeignKey(ct => ct.SenderId);
        }
    }
}
