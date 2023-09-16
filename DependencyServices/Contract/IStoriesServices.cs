using ColllaberaDigital.WebApi.DependencyServices.Dtos;

namespace ColllaberaDigital.WebApi.DependencyServices.Contract
{
    public interface IStoriesServices
    {
        Task<List<long>> GetBestStories(string endpoint, int? n);
        Task<Story> GetBestStoryById(string endpoint);
    }
}
