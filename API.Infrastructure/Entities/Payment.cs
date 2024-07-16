using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace API.Infrastructure.Entities
{
    public class Payment : BaseEntity
    {
        public long TrackId { get; set; }
        public long Amount { get; set; }
        public string? Description { get; set; }
        public string? OrderId { get; set; }
        public int TransactionStatusId { get; set; }
        public long? RefNumber { get; set; }
        public DateTime? PaidAt { get; set; }
        public DateTime? VerifiedAt { get; set; }
        public string? CardNumber { get; set; }
        public TransactionStatus TransactionStatus { get; set; }
    }

    public class PaymentEntityTypeConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasIndex(x => x.TrackId).IsUnique().IsClustered(false);
        }
    }
}
