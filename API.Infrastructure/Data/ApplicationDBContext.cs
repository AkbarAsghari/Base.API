using API.Infrastructure.Entities;
using API.Infrastructure.Extensions;
using API.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using API.Shared.Extensions;

namespace API.Infrastructure.Data
{
    public class ApplicationDBContext : DbContext
    {
        private readonly IHttpContextAccessor _accessor;

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options, IHttpContextAccessor accessor) : base(options)
        {
            _accessor = accessor;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            #region ApplyConfigurations

            builder.ApplyConfiguration(new UsersEntityTypeConfiguration());
            builder.ApplyConfiguration(new ResetPasswordsTicketsEntityTypeConfiguration());
            builder.ApplyConfiguration(new EmailNotificationEntityTypeConfiguration());
            builder.ApplyConfiguration(new EmailStatusEntityTypeConfiguration());
            builder.ApplyConfiguration(new EmailTypeEntityTypeConfiguration());
           


            #endregion

            ApplyQueryFilters(builder);

            base.OnModelCreating(builder);
        }

        private void ApplyQueryFilters(ModelBuilder builder)
        {
            var clrTypes = builder.Model.GetEntityTypes().Select(et => et.ClrType).ToList();

            foreach (var type in clrTypes)
            {
                if (typeof(IUserEntity).IsAssignableFrom(type))
                    builder.Entity(type).AddQueryFilter<IUserEntity>(e => e.UserId == _accessor.HttpContext.GetClaimsUserID());
                if (typeof(ISoftDeleteEntity).IsAssignableFrom(type))
                    builder.Entity(type).AddQueryFilter<ISoftDeleteEntity>(e => e.IsDeleted == false);
            }
        }
    }
}
