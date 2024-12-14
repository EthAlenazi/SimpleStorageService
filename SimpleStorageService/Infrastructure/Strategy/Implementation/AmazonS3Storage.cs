using Application;
using Core;
using Infrastructure.Strategy.Interface;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Strategy.Implementation
{
    public class AmazonS3Storage : IStorageStrategy
    {
        
        private static readonly string bucketName = "my-simple-drive-demo";
        private static readonly string region = "eu-north-1";
        private static readonly string service = "s3";
        private static readonly string endpoint = $"https://{bucketName}.s3.{region}.amazonaws.com";
        private static readonly string objectName = "C:\\Sorce\\Test\\ConsoleApp1\\Test.txt";
        private static readonly string accessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY");
        private static readonly string secretKey =Environment.GetEnvironmentVariable("AWS_SECRET_KEY");
        public async Task<string> UploadFileAsync(string objectContent, Guid fileId)
        {
            try
            {
                if (string.IsNullOrEmpty(accessKey) || string.IsNullOrEmpty(secretKey))
                    throw new InvalidOperationException("AWS credentials are not set.");

                string contentType = CommonServices.GetContentType(objectContent);
                string date = DateTime.UtcNow.ToString("yyyyMMdd");
                string datetime = DateTime.UtcNow.ToString("yyyyMMdd'T'HHmmss'Z'");

                // Convert file content to bytes
                byte[] fileBytes = Encoding.UTF8.GetBytes(objectContent);

                // Generate the canonical URI and headers
                string canonicalUri = $"/{fileId}";
                string canonicalHeaders = $"content-type:{contentType}\n" +
                                           $"host:{bucketName}.s3.{region}.amazonaws.com\n" +
                                           $"x-amz-content-sha256:{CommonServices.HexEncode(
                                               CommonServices.SHA256Hash(objectContent))}\n" +
                                           $"x-amz-date:{datetime}\n";
                string signedHeaders = "content-type;host;x-amz-content-sha256;x-amz-date";

                // Generate the canonical request
                string canonicalRequest = $"PUT\n" +
                                           $"{canonicalUri}\n" +
                                           $"\n{canonicalHeaders}\n" +
                                           $"{signedHeaders}\n" +
                                           $"{CommonServices.HexEncode(
                                              CommonServices.SHA256Hash(objectContent))}";

                // Generate the string to sign
                string stringToSign = $"AWS4-HMAC-SHA256\n" +
                                      $"{datetime}\n" +
                                      $"{date}/{region}/{service}/aws4_request\n" +
                                      $"{CommonServices.HexEncode(
                                         CommonServices.SHA256Hash(canonicalRequest))}";

                // Generate the signature
                byte[] signingKey = CommonServices.GetSignatureKey(secretKey, date, region, service);
                string signature = CommonServices.HexEncode(
                    CommonServices.HmacSHA256(stringToSign, signingKey));

                // Create the authorization header
                string authorizationHeader = $"AWS4-HMAC-SHA256 Credential={accessKey}/{date}/{region}/{service}/aws4_request, " +
                                             $"SignedHeaders={signedHeaders}, Signature={signature}";

                // Create the request
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"{endpoint}/{fileId}");
                request.Method = "PUT";
                request.ContentType = contentType;
                request.ContentLength = fileBytes.Length;
                request.Headers.Add("Authorization", authorizationHeader);
                request.Headers.Add("x-amz-date", datetime);
                request.Headers.Add("x-amz-content-sha256", CommonServices.HexEncode(
                    CommonServices.SHA256Hash(objectContent)));

                // Write the file content to the request stream
                using (Stream requestStream = request.GetRequestStream())
                {
                    await requestStream.WriteAsync(fileBytes, 0, fileBytes.Length);
                }

                // Get the response
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return "File uploaded successfully to Amazon storages";

                    }
                    else
                    {
                        return $"Failed to upload file. Status code: {response.StatusCode} {response.StatusDescription}";
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Error uploading file: {ex.Message}";
            }
        }
        public async Task<OutputModel> DownloadFileAsync(string fileId)
        {
            try
            {
                if (string.IsNullOrEmpty(accessKey) || string.IsNullOrEmpty(secretKey))
                    throw new InvalidOperationException("AWS credentials are not set.");

                string date = DateTime.UtcNow.ToString("yyyyMMdd");
                string datetime = DateTime.UtcNow.ToString("yyyyMMdd'T'HHmmss'Z'");
                string canonicalUri = $"/{fileId}";
                string host = $"{bucketName}.s3.{region}.amazonaws.com";

                // Canonical request
                string canonicalHeaders = $"host:{host}\n" +
                                           $"x-amz-content-sha256:UNSIGNED-PAYLOAD\n" +
                                           $"x-amz-date:{datetime}\n";
                string signedHeaders = "host;x-amz-content-sha256;x-amz-date";

                string canonicalRequest = $"GET\n" +
                                           $"{canonicalUri}\n" +
                                           $"\n{canonicalHeaders}\n" +
                                           $"{signedHeaders}\n" +
                                           "UNSIGNED-PAYLOAD";

                // String to sign
                string stringToSign = $"AWS4-HMAC-SHA256\n" +
                                      $"{datetime}\n" +
                                      $"{date}/{region}/{service}/aws4_request\n" +
                                      $"{CommonServices.HexEncode(
                                        CommonServices.SHA256Hash(canonicalRequest))}";

                // Signature
                byte[] signingKey = CommonServices.GetSignatureKey(secretKey, date, region, service);
                string signature = CommonServices.HexEncode(
                    CommonServices.HmacSHA256(stringToSign, signingKey));

                // Authorization header
                string authorizationHeader = $"AWS4-HMAC-SHA256 Credential={accessKey}/{date}/{region}/{service}/aws4_request, " +
                                             $"SignedHeaders={signedHeaders}, Signature={signature}";

                // Create HTTP GET request
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"https://{host}/{fileId}");
                request.Method = "GET";
                request.Headers.Add("Authorization", authorizationHeader);
                request.Headers.Add("x-amz-date", datetime);
                request.Headers.Add("x-amz-content-sha256", "UNSIGNED-PAYLOAD");

                // Get the response
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        string content;
                        string contentType = response.ContentType;
                        long contentLength = response.ContentLength;
                        string lastModified = response.Headers["Last-Modified"];

                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            content = await reader.ReadToEndAsync();
                        }

                        return new OutputModel
                        {
                            Id = fileId.ToString(),
                            Data = content, 
                            Size = (int)contentLength,
                            Created_at = DateTime.Parse(lastModified) // 
                        };
                    }
                    else
                    {
                        throw new Exception($"Failed to retrieve file. Status code: {response.StatusCode}");
                    }
                }
            }
            catch (WebException webEx)
            {
                using (var stream = webEx.Response?.GetResponseStream())
                using (var reader = stream != null ? new StreamReader(stream) : null)
                {
                    string errorResponse = reader?.ReadToEnd();
                    Console.WriteLine($"Error response: {errorResponse}");
                }
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error downloading file: {ex.Message}");
                throw;
            }
        }


    }
}
