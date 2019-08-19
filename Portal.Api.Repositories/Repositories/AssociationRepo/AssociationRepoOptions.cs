using Portal.Api.Repositories.Contracts;

namespace Portal.Api.Repositories.Repos
{
    public class AssociationRepoOptions : IAssociationRepoOptions
    {
        public string CacheItemName { get; set; }
    }
}
