using Core;
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
            var Id = new Guid(fileId);
            var fileContent = await _dbContext.ObjectContent.FirstOrDefaultAsync(f => f.Id == Id);

            var fileMetadata = await _dbContext.ObjectMetadata
                .Include(fm => fm.Content)
                .FirstOrDefaultAsync(fm => fm.FileName == fileId);

            if (fileMetadata == null || fileMetadata.Content == null)
            {
                throw new FileNotFoundException("File not found in the database.");
            }
            var result = new OutputModel
            {
                Created_at = fileMetadata.UploadDate,
                Data = fileContent.Content.ToString(),
                Size = (int)fileMetadata.Size,
                Id = fileMetadata.Id.ToString()
            };
           return result;
        }
    }
}
