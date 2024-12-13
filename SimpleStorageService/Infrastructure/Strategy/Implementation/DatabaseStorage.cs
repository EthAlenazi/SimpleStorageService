﻿using Core;
using Core.Database;
using Infrastructure.Strategy.Interface;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Strategy.Implementation
{
    public class DatabaseStorage: IStorageStrategy
    {
        private readonly SystemStorageDbContext _dbContext;

        public DatabaseStorage(SystemStorageDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task UploadFileAsync(string fileContent, Guid fileId)
        {
            var DataContent = new ObjectContent
            {
                Id = fileId,
                Content = fileContent
            };

            var fileMetadata = new ObjectMetadata
            {
                Id= fileId,
                FileName = fileId.ToString(),
                Size = DataContent.Content.Length,
                UploadDate = DateTime.UtcNow,
                Content = DataContent
            };

            await _dbContext.ObjectMetadata.AddAsync(fileMetadata);
            await _dbContext.ObjectContent.AddAsync(DataContent);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<OutputModel> DownloadFileAsync(string fileId)
        {
            try
            {
                var Id= new Guid(fileId);
                //var fileContent = await _dbContext.ObjectContent.FirstOrDefaultAsync(f => f.Id == Id);
                //if (fileContent == null)
                //{
                //    throw new FileNotFoundException("File content not found in the database.");
                //}

                // Ensure fileMetadata is fetched correctly
                var fileMetadata = await _dbContext.ObjectMetadata
                    .Include(fm => fm.Content)
                    .FirstOrDefaultAsync(fm => fm.Id == Id);
                if (fileMetadata == null || fileMetadata.Content == null)
                {
                    throw new FileNotFoundException("File metadata or content not found in the database.");
                }
                var result = new OutputModel();

                result.Created_at = fileMetadata.UploadDate;
                result.Data = fileMetadata.Content.Content;
                result.Size = Convert.ToInt32(fileMetadata.Size);
                result.Id = fileMetadata.Id.ToString();
                
                return result;
            }
            catch (Exception ex) {
                throw ex;

            }
        }
    }
}
