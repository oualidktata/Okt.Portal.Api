using System.Collections.Generic;
using Assette.Client;
using Framework.Common;
using SieveModel = Sieve.Models.SieveModel;

namespace Portal.Api.Repositories.Contracts
{
    public interface IDocumentRepository
    {
        ResultObj<DocumentDto> CreateDocument(DocumentDto documentDto);
        ResultObj<DocumentDto> Find(DocumentDto documentDto);
        ResultObj<DocumentDto> Find(string originalFileName);
        ResultObj<IEnumerable<DocumentDto>> FindAll();
        IEnumerable<DocumentDto> GetDocuments(SieveModel searchModel);
    }
}