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
    public class EmailType
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class EmailTypeEntityTypeConfiguration : IEntityTypeConfiguration<EmailType>
    {
        public void Configure(EntityTypeBuilder<EmailType> builder)
        {
            builder.HasData(new EmailType[]
            {
                new EmailType { Id = 1, Name = "BackUp"},
                new EmailType { Id = 2, Name = "ConfirmEmail"},
                new EmailType { Id = 3, Name = "ForgetPassword"},
                new EmailType { Id = 4, Name = "ResendActivationLink"},
                new EmailType { Id = 5, Name = "AnswerTicket"},
            });
        }
    }
}
