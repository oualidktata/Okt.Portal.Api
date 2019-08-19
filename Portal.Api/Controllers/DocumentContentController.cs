using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.Annotations;
using Portal.Api.Repositories.Contracts;
using System.Linq;
using System.IO;
using Assette.Client;
using Framework.Common;

namespace Portal.Api.Controllers
{
    [Route("api/v{version:apiVersion}/doccontent")]
    [Produces("application/json", "application/xml")]
    [Consumes("application/json", "application/xml")]
    [ApiController]
    public class DocumentContentController : ControllerBase
    {
        private ICachebaleDocMetaDataRepository _documentMetadataRepo { get; set; }

        public DocumentContentController(ICachebaleDocMetaDataRepository _documentRepository)
        {
            _documentMetadataRepo = _documentRepository;
        }

        /// <summary>
        /// Uploads a document. Need to create document metadata using /api/v1/documentmetadata beforehand.
        /// </summary>
        /// <param name="documentId">The Key of the document.</param>
        /// <param name="fileName">Document To upload</param>
        /// <returns>The created Document</returns>
        [SwaggerResponse(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        [Produces("application/json", "text/plain", "application/octet-stream", "application/xml")]
        [Consumes("application/octet-stream", "application/json", "application/xml", "text/plain")]
        [HttpPost("{documentId}")]
        [SwaggerOperation(
           Description = @"Requires admin privileges. Need to create document metadata using
                            '/api/v1/documentmetadata' before uploding the document",
           OperationId = "UploadDocument",
           Tags = new[] { "Document Content" }
       )]
        public async Task<ActionResult> UploadDocument(string documentId, [FromForm] IFormFile fileName)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var results = new List<ResultObj<DocumentDto>>(); //_documentMetadataRepo.CreateOrUpdateEntities(fileToUpload);
                if (results.Any(r => !r.Success))
                {
                    return new ConflictObjectResult(results);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode(StatusCodes.Status500InternalServerError, ex.Message));
            }
        }

        /// <summary>
        /// Gets a document.
        /// </summary>
        /// <param name="documentId">The Key of the document.</param>
        /// <returns>The Document if found.</returns>
        [HttpGet("{documentId}")]
        [ProducesResponseType(typeof(IFormFile), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        [Produces("application/octet-stream")]
        [Consumes("application/octet-stream", "application/json", "application/xml", "text/plain")]
        [SwaggerOperation(
            Description = "Requires admin privileges",
            OperationId = "DownloadDocument",
            Tags = new[] { "Document Content" }
        )]
        public async Task<ActionResult<IFormFile>> DownloadDocument(string documentId)
        {
            try
            {
                var Document = _documentMetadataRepo.FindByKey(doc => doc.DocumentId == documentId);
                if (Document == null)
                {
                    return new ConflictObjectResult("The Document does not exist");
                }
                var stream = new FileStream("whatever", FileMode.Create);
                return File(stream, "application/octet-stream");
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode(StatusCodes.Status500InternalServerError, ex.Message));
            }
        }

        /// <summary>
        /// Uploads documents. Need to create document metadata using /api/v1/documentmetadata beforehand.
        /// </summary>
        /// <param name="documentId">The Key of the document.</param>
        /// <param name="fileName">Document To upload</param>
        /// <returns>The created Document</returns>
        [SwaggerResponse(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        [Produces("application/json", "text/plain", "application/octet-stream", "application/xml")]
        [Consumes("application/octet-stream", "application/json", "application/xml", "text/plain")]
        [HttpPost("bulkupload")]
        [SwaggerOperation(
           Description = @"Requires admin privileges. Need to create document metadata using
                            '/api/v1/documentmetadata' before uploding the document",
           OperationId = "BulkUpload",
           Tags = new[] { "Document Content" }
       )]
        public async Task<ActionResult> BulkUpload(string documentId, [FromForm] IFormFile[] fileName)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var results = new List<ResultObj<DocumentDto>>(); //_documentMetadataRepo.CreateOrUpdateEntities(fileToUpload);
                if (results.Any(r => !r.Success))
                {
                    return new ConflictObjectResult(results);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode(StatusCodes.Status500InternalServerError, ex.Message));
            }
        }

        /// <summary>
        /// Gets a document.TODO decide whether:string[] documentIds or Comma separated Keys of the documents or RequestBody.
        /// </summary>
        /// <param name="documentIds">document Ids.</param>
        /// <returns>The Document if found.</returns>
        [HttpGet("bulkdownload")]
        [ProducesResponseType(typeof(IFormFile), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        [Produces("application/octet-stream")]
        [Consumes("application/octet-stream", "application/json", "application/xml", "text/plain")]
        [SwaggerOperation(
            Description = "Requires admin privileges. TODO decide whether:string[] documentIds or Comma separated Keys of the documents or RequestBody.",
            OperationId = "BulkDownload",
            Tags = new[] { "Document Content" }
        )]

        public async Task<ActionResult<IFormFile>> BulkDownload([FromQuery(Name = "documentIds")] string[] documentIds)
        {
            try
            {
                //TODO Untested
                var Document = _documentMetadataRepo.FindByKey(doc => doc.DocumentId == documentIds[0]);
                if (Document == null)
                {
                    return new ConflictObjectResult("The Document does not exist");
                }
                var stream = new FileStream("whatever", FileMode.Create);
                return File(stream, "application/octet-stream");
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode(StatusCodes.Status500InternalServerError, ex.Message));
            }
        }


    }
}