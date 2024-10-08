﻿using API.Infrastructure.DataAcess;
using API.Infrastructure.Data;
using API.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using API.Shared;

namespace API.Infrastructure.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDBContextDependencies(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDBContext>(options =>
            {
                options.UseSqlServer(AppSettiungs.ApplicationDBContextConnectionString);
            });

            services.AddScoped<DbContext, ApplicationDBContext>();

            services.AddTransient<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
