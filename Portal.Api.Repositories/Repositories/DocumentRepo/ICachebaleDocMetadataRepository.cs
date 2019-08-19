using Assette.Client;
using Framework.Common;
using Portal.Api.Repositories.Repos;

namespace Portal.Api.Repositories.Contracts
{

    public interface ICachebaleDocMetaDataRepository : ICachebaleRepository<DocumentDto, DocumentDto, DocumentSimpleDto>
    {
        /// <summary>
        /// Removes a document metadata
        /// </summary>
        /// <param name="documentId">identifier of the document</param>
        /// <returns></returns>
        ResultObj<DocumentSimpleDto> RemoveDocumentMetaData(string documentId);
    }
}