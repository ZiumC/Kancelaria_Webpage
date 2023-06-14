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

        /// <summary>
        /// This method allows to select status code from dictorany initialized in any class
        /// </summary>
        /// <param name="statusCode">Status code as int.</param>
        /// <param name="statuses"> Dictionary<int, string></param>
        /// <returns>Selected value based on passed <paramref name="statusCode"/></returns>
        public static Tuple<int, string> SelectStatusBy(int statusCode, Dictionary<int, string> statuses)
        {
            var selectedStatus = statuses
                        .Where(s => s.Key == statusCode)
                        .FirstOrDefault();

            return Tuple.Create(selectedStatus.Key, selectedStatus.Value);
        }
    }
}
