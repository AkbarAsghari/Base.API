using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace API.Infrastructure.Entities
{
    public class PaymentStatus
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
    }

    public class PaymentStatusEntityTypeConfiguration : IEntityTypeConfiguration<PaymentStatus>
    {
        public void Configure(EntityTypeBuilder<PaymentStatus> builder)
        {
            builder.HasData(new List<PaymentStatus>
            {
                new PaymentStatus { Id = -1 , Description = "Awaiting payment" },
                new PaymentStatus { Id = -2 , Description = "Internal error" },
                new PaymentStatus { Id = 1 , Description = "Paid - Confirmed" },
                new PaymentStatus { Id = 2 , Description = "Paid - Unverified" },
                new PaymentStatus { Id = 3 , Description = "Canceled by user" },
                new PaymentStatus { Id = 4 , Description = "The card number is invalid" },
                new PaymentStatus { Id = 5 , Description = "The account balance is insufficient" },
                new PaymentStatus { Id = 6 , Description = "The entered password is wrong" },
                new PaymentStatus { Id = 7 , Description = "The number of requests is over the limit" },
                new PaymentStatus { Id = 8 , Description = "The number of online payments per day is more than the allowed limit" },
                new PaymentStatus { Id = 9 , Description = "The amount of daily internet payment is more than the allowed limit" },
                new PaymentStatus { Id = 10 , Description = "The issuer of the card is invalid" },
                new PaymentStatus { Id = 11 , Description = "Switch error" },
                new PaymentStatus { Id = 12 , Description = "The card is not accessible" },
            });
        }
    }
}
