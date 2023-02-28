using GB_Webpage.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace GB_Webpage.Services
{
    public class HttpService
    {

        public static async Task<T?> MakeGetRequest<T>(string url)
        {

            using (var client = new HttpClient())
            {
                Uri uri = new Uri(url);

                HttpResponseMessage response = await client.GetAsync(uri);

                string responseResult = await response.Content.ReadAsStringAsync();

                if (responseResult != null)
                {
                    return JsonConvert.DeserializeObject<T>(responseResult);
                }
                else
                {
                    return default;
                }

            }

        }
    }
}
