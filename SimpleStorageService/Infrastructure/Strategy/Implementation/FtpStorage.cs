using Core;
using FluentFTP;
using Infrastructure.Strategy.Interface;

namespace Infrastructure.Strategy.Implementation
{
    public class FtpStorage : IStorageStrategy
    {
        
        string host =     Environment.GetEnvironmentVariable("FTP_HOST_KEY");//FTP IP
        string username = Environment.GetEnvironmentVariable("FTP_USERNAME_KEY"); // FTP username
        string password = Environment.GetEnvironmentVariable("AWS_PASSWORD_KEY");// FTP password
        string localFilePath = @"D:\Test\File.txt";// Local file path
        string remoteDirectory = "/htdocs";// Remote directory path


        public async Task UploadFileAsync(string fileContent, Guid fileId)
        {
            // Create an FTP client
            using (var client = new AsyncFtpClient(host, username, password))
            {
                try
                {
                    await client.AutoConnect();

                    string remoteFilePath = $"{remoteDirectory}/{fileId}_{Path.GetFileName(localFilePath)}";

                    client.Config.RetryAttempts = 3;
                    await client.UploadFile(localFilePath, remoteFilePath, FtpRemoteExists.Overwrite, true, FtpVerify.Retry);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    await client.Disconnect();
                }
            }
        }
        public async Task<OutputModel> DownloadFileAsync(string fileId)
        {
            OutputModel result = new OutputModel();
           using (var client = new AsyncFtpClient(host, username, password))
            {
                try
                {
                    await client.AutoConnect();

                    string remoteFilePath = $"{remoteDirectory}/{fileId}_{Path.GetFileName(localFilePath)}";

                    
                    string retrievedFilePath = $@"D:\Test\Result\{Path.GetFileName(localFilePath)}"; 
                    await client.DownloadFile(retrievedFilePath, remoteFilePath);

                    Console.WriteLine($"File retrieved successfully to: {retrievedFilePath}");
                    result.Data = retrievedFilePath;
                    result.Size = retrievedFilePath.Length;
                    result.Created_at = DateTime.Now;
                    result.Id = fileId;
                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    
                    await client.Disconnect();
                    
                }
            }

        }
    }
}
