using Core;
using Core.Database;
using Infrastructure.Factory;
using Infrastructure.Strategy.Implementation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyInjection
{
    public static class ConfigurationFile
    {
        public static IServiceCollection ReadConfigurationFiles(this IServiceCollection services, IConfiguration configuration)
        {


            // Add specific storage services to DI
            services.AddScoped<AmazonS3Storage>();
            services.AddScoped<DatabaseStorage>();
            services.AddScoped<LocalFileSystemStorage>();
            services.AddScoped<FtpStorage>();

            // Register the StorageFactory
            services.AddScoped<StorageProviderFactory>();

            // Register StorageHandler with dynamic storages
            services.AddScoped<StorageServiceHandler>(serviceProvider =>
            {
                var factory = serviceProvider.GetRequiredService<StorageProviderFactory>();
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();

                // Fetch storage types from configuration or use a default list
                var storageTypes = configuration.GetValue<string>("StorageSettings:EnabledTypes")?? new ( "Database") ;

                // Create storages dynamically
                var storages = factory.CreateStorages(storageTypes);
                return new StorageServiceHandler(storages);
            });

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<SystemStorageDbContext>(options =>
                options.UseSqlServer(connectionString));
            return services;

        }
    }
}
