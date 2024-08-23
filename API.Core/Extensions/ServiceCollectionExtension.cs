using API.Core.Configurations;
using API.Core.Interfaces.Services;
using API.Core.Services;
using API.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Quartz;
using API.Core.ScheduleJobs;
using Microsoft.AspNetCore.Authorization;

namespace API.Core.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCoreDependencies(this IServiceCollection services)
        {

            //JWT
            var key = Encoding.ASCII.GetBytes(JWTSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Weather.View",
                    policy => policy.RequireAssertion(x => AssertClaim(x, "Weather.View")));
            });

            static bool AssertClaim(AuthorizationHandlerContext context, string claim) => context.User.HasClaim(x => x.Type == "Permissions" && x.Value == claim);

            //AutoMapper
            services.AddAutoMapper(typeof(AutoMapperProfile));

            //Services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmailNotificationService, EmailNotificationService>();
            services.AddScoped<IPaymentService, PaymentService>();


            //Quarts Scheduler
            services.AddQuartz(q =>
            {
                // these are the defaults
                q.UseSimpleTypeLoader();
                q.UseInMemoryStore();
                q.UseDefaultThreadPool(tp =>
                {
                    tp.MaxConcurrency = 1;
                });

                //Payments
                q.ScheduleJob<InquiryPaymentsStatusJob>(trigger => trigger
                .StartNow().WithSimpleSchedule(x => x.WithIntervalInMinutes(60).RepeatForever()));

            });

            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);


            return services;
        }
    }
}
