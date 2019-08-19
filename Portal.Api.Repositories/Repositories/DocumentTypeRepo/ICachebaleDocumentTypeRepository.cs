using Assette.Client;
using Portal.Api.Repositories.Repos;

namespace Portal.Api.Repositories.Contracts
{

    public interface ICachebaleDocumentTypeRepository : ICachebaleRepository<DocumentTypeDto, DocumentTypeToCreateDto, DocumentTypeDto>
    {
        
    }
}