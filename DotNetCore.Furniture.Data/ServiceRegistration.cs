using DotNetCore.Furniture.Data.Repositories.Implementations;
using DotNetCore.Furniture.Data.Repositories.Interfaces;
using DotNetCore.Furniture.Domain.Config.Implementations;
using DotNetCore.Furniture.Domain.Config.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCore.Furniture.Data
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddDataDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            try
            {
                //services.AddScoped<ISampleRepository, SampleRepository>();
                services.AddScoped<IUserRepository, UserRepository>();
                //services.AddScoped<IFurnitureRepository, FurnitureRepository>();
                //services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
                //services.AddScoped<IOrderRepository, OrderRepository>();
                //services.AddScoped<IPreOrderRepository, PreOrderRepository>();
                //services.AddScoped<IWishlistRepository, WishlistRepository>();
                //services.AddScoped<IInventoryRepository, InventoryRepository>();
                //services.AddScoped<ICustomerSupportRepository, CustomerSupportRepository>();
                services.AddSingleton<IMongoClient, MongoClient>(sp => new MongoClient(configuration["MongoDbSettings:ConnectionString"]));
                services.AddSingleton<IMongoDbConfig, MongoDbConfig>(
                    sp => new MongoDbConfig(configuration.GetSection("MongoDbSettings:ConnectionString").Value,
                configuration.GetSection("MongoDbSettings:DatabaseName").Value));
                services.AddScoped<IMongoDBLogContext, MongoDBLogContext>();
                //services.AddScoped<IMyBankLogRepository, MyBankLogRepository>();

                return services;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
