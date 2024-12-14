using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public static class CommonServices
    {
        public static bool IsValidBase64(string base64String)
        {
            if (string.IsNullOrWhiteSpace(base64String))
                return false;

            //length is valid for Base64
            if (base64String.Length % 4 != 0)
                return false;

            try
            {
                Convert.FromBase64String(base64String);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static Stream GetStreamFromString(string text)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(text);
            MemoryStream stream = new MemoryStream(byteArray);
            return stream;
        }

        public static string GetContentType(string filePath)
        {
            return filePath.EndsWith(".png") ? "image/png" : "application/octet-stream";
        }
        public static byte[] HmacSHA256(string data, byte[] key)
        {
            using (var hmac = new HMACSHA256(key))
            {
                return hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            }
        }

        public static byte[] SHA256Hash(string data)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(data));
            }
        }

        public static byte[] GetSignatureKey(string key, string dateStamp, string regionName, string serviceName)
        {
            byte[] kDate = HmacSHA256(dateStamp, Encoding.UTF8.GetBytes("AWS4" + key));
            byte[] kRegion = HmacSHA256(regionName, kDate);
            byte[] kService = HmacSHA256(serviceName, kRegion);
            return HmacSHA256("aws4_request", kService);
        }

        public static string HexEncode(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (var b in bytes)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }

    }
}
