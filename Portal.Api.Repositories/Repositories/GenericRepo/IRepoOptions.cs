namespace Portal.Api.Repositories.Contracts
{
    public interface IRepoOptions
    {
        string CacheItemName { get; set; }
    }
    public class RepoOptions : IRepoOptions
    {
        public string CacheItemName { get; set; }
    }
}
