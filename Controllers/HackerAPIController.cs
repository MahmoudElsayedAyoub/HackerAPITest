using ColllaberaDigital.WebApi.DependencyServices.Contract;
using ColllaberaDigital.WebApi.DependencyServices.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ColllaberaDigital.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HackerAPIController : ControllerBase
    {

        private readonly ILogger<HackerAPIController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IStoriesServices _storiesServices;


        public HackerAPIController(
        ILogger<HackerAPIController> logger,
        IConfiguration configuration,
        IStoriesServices storiesServices)
        {
            _logger = logger;
            _configuration = configuration;
            _storiesServices = storiesServices;
        }


        [HttpGet(nameof(ListBestStories))]
        public async Task<List<long>> ListBestStories()
        {
            return await _storiesServices.GetBestStories("v0/beststories.json", null); ;
        }


        [HttpGet(nameof(GetBestStoriesByN))]
        public async Task<List<long>> GetBestStoriesByN(int? n)
        {
            return await _storiesServices.GetBestStories("v0/beststories.json", n); ;
        }



        // GET api/<ValuesController>/5
        [HttpGet(nameof(GetById))]
        public async Task<Story> GetById(long id)
        {
            return await _storiesServices.GetBestStoryById($"v0/item/{id}.json?print=pretty");
        }
    }
}
