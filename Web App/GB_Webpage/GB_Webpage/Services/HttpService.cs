using GB_Webpage.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace GB_Webpage.Services
{
    public class HttpService
    {

        public static async Task<IEnumerable<T>?> DoGetCollection<T>(string url)
        {

            using (var client = new HttpClient())
            {
                Uri uri = new Uri(url);

                HttpResponseMessage response = await client.GetAsync(uri);

                string responseResult = await response.Content.ReadAsStringAsync();

                if (responseResult != null)
                {
                    return JsonConvert.DeserializeObject<IEnumerable<T>>(responseResult);
                }
                else
                {
                    return default;
                }

            }

        }
    }
}
