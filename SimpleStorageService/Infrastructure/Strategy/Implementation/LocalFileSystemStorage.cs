using Core;
using Infrastructure.Strategy.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Strategy.Implementation
{
    public class LocalFileSystemStorage : IStorageStrategy
    {
       
        private readonly string _storagePath;


        public LocalFileSystemStorage()
        {
            _storagePath = Path.Combine(Directory.GetCurrentDirectory(), "Storage");
            if (!Directory.Exists(_storagePath))
                Directory.CreateDirectory(_storagePath);
        }

        public Task UploadFileAsync(string fileContent, Guid fileId)
        {
            try
            {
                var filePath = Path.Combine(_storagePath, fileId.ToString());
                var data = Convert.FromBase64String(fileContent);
                File.WriteAllBytes(filePath, data);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<OutputModel> DownloadFileAsync(string fileId)
        {

            try
            {
                var filePath = Path.Combine(_storagePath, fileId.ToString());
                if (!File.Exists(filePath))
                    return null;

                var data = File.ReadAllBytes(filePath);
                var createdAt = File.GetCreationTimeUtc(filePath);
                var base64Data = Convert.ToBase64String(data);
                var outputResult = new OutputModel()
                {
                    Id = fileId,
                    Data = base64Data,
                    Created_at = createdAt,
                    Size = data.Length
                };

                return outputResult;
            }
            catch (Exception ex) {
                throw ex;
            }

        }
    }

}
