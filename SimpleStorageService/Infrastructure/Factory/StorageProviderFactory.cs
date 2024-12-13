using Core;
using Core.Database;
using Infrastructure.Strategy.Implementation;
using Infrastructure.Strategy.Interface;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Factory
{
    public class StorageProviderFactory
    {
        private readonly StorageSettings _settings;
        private readonly SystemStorageDbContext _dbContext;

        public StorageProviderFactory(IOptions<StorageSettings> options, SystemStorageDbContext context)
        {
            _settings = options.Value;
            _dbContext = context;
        }
        public IStorageStrategy CreateStorages(string storageTypes = null)
        {
            var type = storageTypes ?? new ( _settings.DefaultStorage );

            return  type switch
            {
                "AmazonS3" => new AmazonS3Storage(),
                "Database" => new DatabaseStorage(_dbContext),
                "LocalFileSystem" => new LocalFileSystemStorage(),
                "FTP" => new FtpStorage(),
                _ => throw new ArgumentException($"Unknown storage type: {type}")
            };
        }

    }
}
