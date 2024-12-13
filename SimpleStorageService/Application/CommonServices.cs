using System;
using System.Collections.Generic;
using System.Linq;
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

    }
}
