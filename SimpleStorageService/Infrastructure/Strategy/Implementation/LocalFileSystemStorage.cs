using Core;
using Infrastructure.Strategy.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Strategy.Implementation
{
    public class LocalFileSystemStorage: IStorageStrategy
    {
        public Task<OutputModel> DownloadFileAsync(string fileId)
        {
            throw new NotImplementedException();
        }

        public Task UploadFileAsync(string objectContent, Guid fileId)
        {
            throw new NotImplementedException();
        }
    }
}
