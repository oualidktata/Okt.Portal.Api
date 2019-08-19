using System;
using System.Collections.Generic;
using Assette.Client;
using AutoMapper;
using Framework.Common;
using Microsoft.Extensions.Caching.Memory;
using Portal.Api.Repositories.Contracts;
using Sieve.Services;
using SieveModel = Sieve.Models.SieveModel;
namespace Portal.Api.Repositories.Repos
{
    public class InMemoryAccountRepository : CachebaleRepository<AccountDto, AccountToCreateDto, AccountSimpleDto>, ICachebaleAccountRepository
    {
        public InMemoryAccountRepository(IMapper mapper, IMemoryCache cache, IAccountRepoOptions repoOptions, ISieveProcessor sieveProcessor) : base(mapper, cache, repoOptions, sieveProcessor)
        {
        }

        public ResultObj<IEnumerable<AccountDto>> FindByFilter(SieveModel searchModel)
        {
            throw new NotImplementedException();
        }
    }
}
