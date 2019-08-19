using Portal.Api.Repositories.Contracts;

namespace Portal.Api.Repositories.InMemoryRepos
{
    public class UserRepoOptions : IUserRepoOptions
    {
        public string CacheItemName { get; set; }
    }
}
