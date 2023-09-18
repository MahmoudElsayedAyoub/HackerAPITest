using ColllaberaDigital.WebApi.DependencyServices.Contract;
using ColllaberaDigital.WebApi.DependencyServices.Dtos;
using Newtonsoft.Json;
using System.Text;

namespace ColllaberaDigital.WebApi.DependencyServices.Services
{
    public class StoriesServices : IStoriesServices
    {

        private readonly HttpClient _httpClient;
        public StoriesServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }




        public async Task<List<long>> GetBestStories(string endpoint, int? n)
        {
            var responseString = await _httpClient.GetStringAsync((_httpClient.BaseAddress + endpoint));
            var stories = JsonConvert.DeserializeObject<List<long>>(responseString);


            if (n.HasValue)
                return stories?.Take(n.Value)?.ToList() ?? new List<long>();
            else
                return stories ?? new List<long>();

        }

        public async Task<Story> GetBestStoryById(string endpoint)
        {

            var responseString = await _httpClient.GetStringAsync((_httpClient.BaseAddress + endpoint));
            var _story = JsonConvert.DeserializeObject<Story>(responseString);
            return _story ?? new Story();

        }



    }
}
