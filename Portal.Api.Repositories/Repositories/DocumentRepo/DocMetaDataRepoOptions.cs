using Portal.Api.Repositories.Contracts;

namespace Portal.Api.Repositories.InMemoryRepos
{
    public class DocMetaDataRepoOptions : IDocMetaDataRepoOptions
    {
        public string CacheItemName { get; set; }
    }
}
