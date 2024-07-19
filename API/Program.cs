
using API.Shared.Extensions;
using API.Infrastructure.Extensions;
using API.Shared;
using API.Core.Extensions;
using API.Shared.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using API.Attributes;
using API.Middlewares;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.Configure<AppSettiungs>(builder.Configuration.GetSection("AllConfigurations"));

            //IP Limit
            builder.Services.AddScoped<ClientIpCheckActionFilter>(container =>
            {
                var loggerFactory = container.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger<ClientIpCheckActionFilter>();

                return new ClientIpCheckActionFilter(AdminSafeListIPs.IPList, logger);
            });

            builder.Services.AddSharedDependencies();
            builder.Services.AddCoreDependencies();
            builder.Services.AddDBContextDependencies();

            var app = builder.Build();


            //Exception Handler
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
