using Microsoft.Extensions.Options;
using Sieve.Models;

namespace Portal.Api.Repositories.Sieve_search
{
    public class SieveCustomOptions : IOptions<SieveOptions>
    {
        public SieveOptions Value => 
            new SieveOptions() {
                CaseSensitive =false,
                DefaultPageSize =10,
                MaxPageSize =1000,
                ThrowExceptions =true
            };
    }
}
