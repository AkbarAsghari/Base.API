using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Infrastructure.Entities
{
    public class Users : BaseEntity
    {
        public string? Username { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Company { get; set; }
        public string? Mobile { get; set; }
        public string Email { get; set; }
        public bool IsEmailApproved { get; set; }
        public int Role { get; set; }
        public bool Active { get; set; }
    }

    public class UsersEntityTypeConfiguration : IEntityTypeConfiguration<Users>
    {
        public void Configure(EntityTypeBuilder<Users> builder)
        {
            builder.HasIndex(x => x.Username);
            builder.HasIndex(x => x.Email).IsUnique();
        }
    }
}
