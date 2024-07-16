using API.Core.Configurations;
using API.Core.Interfaces.Services;
using API.Core.Services;
using DevMark.ApplicationCore.Interfaces.Services;
using DNSLab.ApplicationCore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace API.Core.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCoreDependencies(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperProfile));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmailNotificationService, EmailNotificationService>();
            services.AddScoped<IPaymentService, PaymentService>();

            return services;
        }
    }
}
