using Core;
using Infrastructure.Strategy.Interface;

namespace Infrastructure;

public class StorageServiceHandler
{
    private readonly IStorageStrategy _storages;

    public StorageServiceHandler(IStorageStrategy storages)
    {
        _storages = storages;
    }

    public async Task HandleUploadAsync(ObjectModel model)
    {
        //foreach (var storage in _storages)
        //{
            await _storages.UploadFileAsync(model.Data, model.Id);
        //}
    }

    public async Task<OutputModel> HandleDownloadAsync(string fileId)
    {
        try
        {
            var result = await _storages.DownloadFileAsync(fileId);


            if (result == null)
                throw new Exception("File not found in any storage");

            return result;
        }
        catch
        {
            return null;
        }
    }
}
