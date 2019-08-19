using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.Annotations;
using Portal.Api.Repositories.Contracts;
using System.Linq;
using Assette.Client;
using SieveModel = Sieve.Models.SieveModel;

namespace Portal.Api.Controllers
{
    [Route("api/v{version:apiVersion}/documentmetadata")]
    [Produces("application/json", "application/xml")]
    [Consumes("application/json", "application/xml")]
    [ApiController]
    public class DocumentMetaDataController : ControllerBase
    {
        private ICachebaleDocMetaDataRepository _documentMetadataRepo { get; set; }

        public DocumentMetaDataController(ICachebaleDocMetaDataRepository _documentRepository)
        {
            _documentMetadataRepo = _documentRepository;
    }

        /// <summary>
        /// Creates or update metadata for documents.
        /// </summary>
        /// <param name="docMetaDatas">Document metadata to create or update</param>
        /// <returns>The created Document</returns>
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(DocumentTypeSimpleDtoListResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        [HttpPost()]
        [SwaggerOperation(
           Description = "Requires admin privileges",
           OperationId = "CreateOrUpdateDocumentsMetadata",
           Tags = new[] { "Document Metadata" }
       )]
        public async Task<ActionResult<DocumentTypeSimpleDtoListResult>> CreateOrUpdateDocumentsMetadata([FromBody] DocumentDto[] docMetaDatas)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var results = _documentMetadataRepo.CreateOrUpdateEntities(docMetaDatas);
                
                var assetteResult = new DocumentTypeSimpleDtoListResult();//Convert from Result
                if (results.Any(r => !r.Success))
                {
                    return new ConflictObjectResult(assetteResult);
                }
                return Ok(assetteResult);
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode(StatusCodes.Status500InternalServerError, ex.Message));
            }
        }

        /// <summary>
        /// Gets a document metadata.
        /// </summary>
        /// <param name="documentId">The Key of the document.</param>
        /// <returns>The Document if found.</returns>
        [HttpGet("{documentId}")]
        [ProducesResponseType(typeof(DocumentDtoResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        [SwaggerOperation(
            Description = "Requires admin privileges",
            OperationId = "GetDocumentMetadata",
            Tags = new[] { "Document Metadata" }
        )]
        public async Task<ActionResult<DocumentDtoResult>> GetDocumentMetadata(string documentId)
        {
            try
            {
                var docMetaData = _documentMetadataRepo.FindByKey(doc=>doc.DocumentId==documentId);
                if (docMetaData == null)
                {
                    return new ConflictObjectResult("The Document does not exist");
                }
                return Ok(new DocumentDtoResult());
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode(StatusCodes.Status500InternalServerError, ex.Message));
            }
        }
        /// <summary>
        /// Gets all documents metadata.
        /// </summary>
        /// <returns>List of All Documents</returns>
        [HttpGet("search")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Result object holding", Type = typeof(DocumentDtoListResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Description = "Requires admin privileges",
            OperationId = "SearchDocumentsMetadata",
            Tags = new[] { "Document Metadata" }
        )]
        //TODO Comment on swaggerHub about refactoring
        public async Task<ActionResult<DocumentDtoListResult>> SearchDocumentsMetadata(SieveModel searchModel)
        {
            try
            {
                
                return Ok(new DocumentDtoListResult());//_documentMetadataRepo.SearchFor(searchModel).ConvertTo
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode(StatusCodes.Status500InternalServerError, ex.Message));
            }
        }

        /// <summary>
        /// Removes a document metadata.
        /// </summary>
        /// <param name="documentId">Removes a document metadata.</param>
        /// <returns>Ok if successfully removed</returns>
        [HttpDelete("{documentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Description = "Requires admin privileges",
            OperationId = "RemoveDocumentMetadata",
            Tags = new[] { "Document Metadata" }
        )]
        public async Task<IActionResult> RemoveDocumentMetadata(string documentId)
        {
            try
            {
                var result = _documentMetadataRepo.RemoveDocumentMetaData(documentId);
                if (!result.Success)
                {
                    return new ConflictObjectResult(result);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode(StatusCodes.Status500InternalServerError, ex.Message));
            }
        }
    }
}