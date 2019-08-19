using Assette.Client;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Portal.Api.Repositories.Contracts;
using Sieve.Services;

namespace Portal.Api.Repositories.Repos
{
    public class InMemoryDocumentTypeRepository : CachebaleRepository<DocumentTypeDto, DocumentTypeToCreateDto, DocumentTypeDto>, ICachebaleDocumentTypeRepository
    {
        public InMemoryDocumentTypeRepository(IMapper mapper, IMemoryCache cache, IDocumentTypeRepoOptions repoOptions, ISieveProcessor sieveProcessor) : base(mapper, cache, repoOptions,sieveProcessor)
        {
        }
    }
}
