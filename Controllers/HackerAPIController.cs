using ColllaberaDigital.WebApi.DependencyServices.Contract;
using ColllaberaDigital.WebApi.DependencyServices.Dtos;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace ColllaberaDigital.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HackerAPIController : ControllerBase
    {

        private readonly ILogger<HackerAPIController> _logger;
        private readonly IConfiguration _configuration;
        public readonly IMemoryCache _memoryCache;
        private readonly IStoriesServices _storiesServices;


        public HackerAPIController(
        ILogger<HackerAPIController> logger,
        IConfiguration configuration,
        IStoriesServices storiesServices,
         IMemoryCache memoryCache)
        {
            _logger = logger;
            _configuration = configuration;
            _storiesServices = storiesServices;
            _memoryCache = memoryCache;
        }


        [HttpGet(nameof(ListBestStories))]
        public async Task<List<long>> ListBestStories()
        {
            return await _storiesServices.GetBestStories("v0/beststories.json", null);
        }


        [HttpGet(nameof(GetBestStoriesByN))]
        public async Task<List<long>> GetBestStoriesByN(int? n)
        {
            return await _storiesServices.GetBestStories("v0/beststories.json", n);
        }



        // GET api/<ValuesController>/5
        [HttpGet(nameof(GetById))]
        public async Task<Story> GetById(long id)
        {
            var _storydata = await _storiesServices.GetBestStoryById($"v0/item/{id}.json?print=pretty");
            if (_memoryCache.TryGetValue("story", out Story? CachedStory))
            {
                _memoryCache.Set("story", _storydata);
            }
            return _storydata ?? new Story();
        }

        [HttpGet(nameof(GetBestStoriesWithDeatilsByN))]
        public async Task<List<Story>> GetBestStoriesWithDeatilsByN(int? n)
        {

            var stories = await _storiesServices.GetBestStories("v0/beststories.json", n);
            var tasks = new List<Task>();
            var finalResult = new List<Story>();
            foreach (var item in stories)
            {
                tasks.Add(Task.Run(async () =>
                {
                    var _storydata = await _storiesServices.GetBestStoryById($"v0/item/{item}.json?print=pretty");
                    finalResult.Add(_storydata);
                }));

            }
            await Task.WhenAll(tasks);
            return finalResult ?? new List<Story>();
        }
    }
}
