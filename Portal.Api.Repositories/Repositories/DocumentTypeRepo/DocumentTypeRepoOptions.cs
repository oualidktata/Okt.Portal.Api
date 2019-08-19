using Portal.Api.Repositories.Contracts;

namespace Portal.Api.Repositories.InMemoryRepos
{
    public class DocumentTypeRepoOptions : IDocumentTypeRepoOptions
    {
        public string CacheItemName { get; set; }
    }
}
