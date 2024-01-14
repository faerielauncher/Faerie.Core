using Faerie.Core.Data;
using System.Text.Json;

namespace Faerie.Core.Http
{
    internal class HttpFactory
    {
        private static readonly HttpClient HttpClient = new();
        private readonly string url;
        private HttpMethod method = HttpMethod.Get;

        public HttpFactory(string url)
        {
            this.url = url;
        }
        public HttpFactory SetMethod(HttpMethod method)
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
        public async Task<(bool, string)> CreateRequestDownload(FaerieDirectory dir)
        {
            try
            {
                string outputPath = Path.Combine(dir.GetPath(), RandomString(8));
                HttpResponseMessage response = await HttpClient.GetAsync(url);

                IEnumerable<string>? values;
                if (response.Headers.TryGetValues("Content-Disposition", out values))
                {
                    if(values is not null)
                    {
                        outputPath = Path.Combine(dir.GetPath(), values.First());
                    }
                }

                using (Stream stream = await HttpClient.GetStreamAsync(url))
                {
                    

                    using (FileStream output = new FileStream(dir.GetPath(), FileMode.Create))
                    {
                        await output.CopyToAsync(stream);
                    }
                }

                if (!File.Exists(outputPath))
                {
                    return (false, "");
                }

                return (true, outputPath);
            } 
            catch (Exception)
            {
                return (false, "");
            }

            
        }

    }
}
