
using Portal.Api.Repositories.Contracts;

namespace Portal.Api.Repositories.Repos
{
    public class AccountRepoOptions : IAccountRepoOptions
    {
        public string CacheItemName { get; set; }
    }
}
