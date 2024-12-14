using Core;

namespace Infrastructure.Strategy.Interface
{
    public interface IStorageStrategy
    {
        Task<string> UploadFileAsync(string objectContent, Guid fileId);
        Task<OutputModel> DownloadFileAsync(string fileId);

    }
}
