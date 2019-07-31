using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Toolbox.Portable.Services
{
    public static class WebImageCache
    {
        static HttpClient downloadClient = new HttpClient();

        public static async Task<string> RetrieveImage(string imageUrl, string fileName = null)
        {
            var storedFilename = string.IsNullOrEmpty(fileName) ? imageUrl : fileName;

            byte[] imageBytes;

            if (!FileSystem.DocumentStorage.FileExists(imageUrl))
            {
                imageBytes = await downloadClient.GetByteArrayAsync(new Uri(imageUrl));

                using (var stream = FileSystem.DocumentStorage.CreateOutputStream(storedFilename))
                {
                    await stream.WriteAsync(imageBytes, 0, imageBytes.Length);

                    await stream.FlushAsync();

                    stream.Dispose();
                }
            }

            return $"{FileSystem.DocumentStorage.BasePath}/{storedFilename}";
        }
    }
}
