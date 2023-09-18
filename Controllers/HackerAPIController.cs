using ColllaberaDigital.WebApi.DependencyServices.Contract;
using ColllaberaDigital.WebApi.DependencyServices.Dtos;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using System.Threading.Tasks;

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
        [HttpGet(nameof(GetStoriesById))]
        public async Task<Story> GetStoriesById(long id)
        {
            //search first in cache 
            var field = typeof(MemoryCache).GetProperty("EntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance);
            var collection = field.GetValue(_memoryCache);
            if (_memoryCache.TryGetValue(id, out Story _Cachestorydata))
            {
                return _Cachestorydata;
            }
            //Get data 
            var _storydata = await _storiesServices.GetBestStoryById($"v0/item/{id}.json?print=pretty");
            //set cache options
            var cacheOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(30));
            //set value
            _memoryCache.Set(_storydata?.id, _storydata, cacheOptions);

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



        [HttpGet(nameof(GetBestStorieswithDeatilsV2ByN))]
        public async Task<List<Story>> GetBestStorieswithDeatilsV2ByN(int? n)
        {

            var stories = await _storiesServices.GetBestStories("v0/beststories.json", n);
            var finalResult = new List<Story>();

            ParallelOptions parallelOptions = new()
            {
                MaxDegreeOfParallelism = 3
            };
            await Parallel.ForEachAsync(stories, parallelOptions, async (Id, token) =>
            {
                var _storydata = await _storiesServices.GetBestStoryById($"v0/item/{Id}.json?print=pretty");
                finalResult.Add(_storydata);
            });

            return finalResult ?? new List<Story>();
        }
    }
}
