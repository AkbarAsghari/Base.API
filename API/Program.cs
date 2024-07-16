
using API.Shared.Extensions;
using API.Infrastructure.Extensions;
using API.Shared;
using API.Core.Extensions;
using API.Shared.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

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

            builder.Services.AddSharedDependencies();
            builder.Services.AddCoreDependencies();
            builder.Services.AddDBContextDependencies();

            var app = builder.Build();


            //Exception Handler
            app.UseExceptionHandler(c => c.Run(async context =>
            {
                var exception = context.Features
                    .Get<IExceptionHandlerPathFeature>()!
                    .Error;

                if (exception is BaseException)
                {
                    context.Response.StatusCode = (int)((BaseException)exception).HttpStatusCode;
                    await context.Response.WriteAsJsonAsync(((BaseException)exception).GenerateResponse());
                }
                else
                {
                    await context.Response.WriteAsJsonAsync(new { Error = "Not_Handler_Exception"});
                }
            }));


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
