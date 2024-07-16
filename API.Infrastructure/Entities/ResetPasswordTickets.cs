using API.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Infrastructure.Entities
{
    public class ResetPasswordTickets : BaseEntity, IUserEntity
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsTokenUsed { get; set; }

        public Users User { get; set; }
    }

    public class ResetPasswordsTicketsEntityTypeConfiguration : IEntityTypeConfiguration<ResetPasswordTickets>
    {
        public void Configure(EntityTypeBuilder<ResetPasswordTickets> builder)
        {

        }
    }
}
