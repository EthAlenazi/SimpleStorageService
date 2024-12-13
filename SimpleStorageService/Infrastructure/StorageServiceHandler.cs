using Core;
using Infrastructure.Strategy.Interface;

namespace Infrastructure;

public class StorageServiceHandler
{
    private readonly IEnumerable<IStorageStrategy> _storages;

    public StorageServiceHandler(IEnumerable<IStorageStrategy> storages)
    {
        _storages = storages;
    }

    public async Task HandleUploadAsync(ObjectModel model)
    {
        foreach (var storage in _storages)
        {
            await storage.UploadFileAsync(model.Data, model.Id);
        }
    }

    public async Task<OutputModel> HandleDownloadAsync(string fileId)
    {
        var downloadTasks = _storages.Select(storage =>
        {
            return Task.Run(async () =>
            {
                try
                {
                    return await storage.DownloadFileAsync(fileId);
                }
                catch
                {
                    return null;
                }
            });
        });

        var completedTask = await Task.WhenAny(downloadTasks);
        var result = await completedTask;

        if (result == null)
            throw new Exception("File not found in any storage");

        return result;
    }
}
