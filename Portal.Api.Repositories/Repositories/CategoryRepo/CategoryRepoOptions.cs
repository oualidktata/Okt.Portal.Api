using Portal.Api.Repositories.Contracts;

namespace Portal.Api.Repositories.InMemoryRepos
{
    public class CategoryRepoOptions : ICategoryRepoOptions
    {
        public string CacheItemName { get; set; }
    }
}
