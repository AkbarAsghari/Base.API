using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Infrastructure.Entities
{
    public class EmailStatus
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class EmailStatusEntityTypeConfiguration : IEntityTypeConfiguration<EmailStatus>
    {
        public void Configure(EntityTypeBuilder<EmailStatus> builder)
        {
            builder.HasData(new EmailStatus[]
            {
                new EmailStatus { Id = 1, Name = "MustBeSend"},
                new EmailStatus { Id = 2, Name = "Sent"},
                new EmailStatus { Id = 3, Name = "UnableToSend"},
            });
        }
    }
}
