using Faerie.Core.Data;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Faerie.Core.Http
{
    internal class FaerieHttpFactory
    {
        /// <summary>
        /// TODO: Multi-threadded downloads / concurrent
        /// TODO: Retry after download fails
        /// </summary>
        private static readonly HttpClient HttpClient = new(new HttpClientHandler()
        {
            AllowAutoRedirect = true
        })
        {
            Timeout = TimeSpan.FromSeconds(120)
        };
        private readonly string url;
        private HttpMethod method = HttpMethod.Get;

        public FaerieHttpFactory(string url)
        {
            this.url = url;
        }
        public FaerieHttpFactory SetMethod(HttpMethod method)
        {
            this.method = method;
            return this;
        }

        public async Task<HttpResponseMessage> CreateRequest()
        {
            HttpRequestMessage request = new HttpRequestMessage(method, url);
            return await HttpClient.SendAsync(request);
        }
        public async Task<T?> CreateRequestAsJson<T>()
        {
            HttpRequestMessage request = new HttpRequestMessage(method, url);
            HttpResponseMessage response = await HttpClient.SendAsync(request);

            return JsonSerializer.Deserialize<T>(await response.Content.ReadAsStringAsync());
        }
        public async Task<(bool, string)> CreateRequestDownload(FaerieDirectory dir, string? fileName = null)
        {
            logger.LogInformation($"Downloading: {url}");
            
            string outputPath = Path.Combine(dir.GetPath(), RandomString(16));

            Uri uri = new Uri(url);
            if(uri.Segments.LastOrDefault() is not null)
            {
                // idk how to get rid of null here
                string file = Path.Combine(dir.GetPath(), uri.Segments.LastOrDefault()!);
                if (IsValidFilename(file))
                {
                    outputPath = file;
                }
            }

            if (!string.IsNullOrEmpty(fileName))
            {
                outputPath = Path.Combine(dir.GetPath(), fileName);
            }

            try
            {
                using (Stream stream = await HttpClient.GetStreamAsync(url))
                using (FileStream output = new FileStream(outputPath, FileMode.Create)) {
                    logger.LogInformation($"Writing file to: {outputPath}");
                    await stream.CopyToAsync(output);
                }

                if (!File.Exists(outputPath))
                {
                    return (false, "");
                }

                return (true, outputPath);
            } 
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                File.Delete(outputPath);

                return (false, "");
            }

            
        }

    }
}
