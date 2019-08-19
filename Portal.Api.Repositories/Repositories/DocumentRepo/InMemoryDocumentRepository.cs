using Assette.Client;
using AutoMapper;
using Framework.Common;
using Microsoft.Extensions.Caching.Memory;
using Portal.Api.Repositories.Contracts;
using Sieve.Services;

namespace Portal.Api.Repositories.Repos
{
    public class InMemoryDocMetaDataRepository : CachebaleRepository<DocumentDto, DocumentDto, DocumentSimpleDto>, ICachebaleDocMetaDataRepository
    {
        public InMemoryDocMetaDataRepository(IMapper mapper, IMemoryCache cache, IDocMetaDataRepoOptions repoOptions, ISieveProcessor sieveProcessor) : base(mapper, cache, repoOptions,sieveProcessor)
        {
        }
        ResultObj<DocumentSimpleDto> ICachebaleDocMetaDataRepository.RemoveDocumentMetaData(string documentId)
        {
            //TO DO: Implement
            return new ResultBuilder<DocumentSimpleDto>().Success(new DocumentSimpleDto()).Build();
        }
    }
}
