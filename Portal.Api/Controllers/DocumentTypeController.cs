using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.Annotations;
using Portal.Api.Repositories.Contracts;
using Assette.Client;
using Framework.Common;

namespace Portal.Api.Controllers
{
    [Route("api/v{version:apiVersion}/doctypes")]
    [Produces("application/json", "application/xml")]
    [Consumes("application/json", "application/xml")]
    [ApiController]
    public class DocumentTypeController : ControllerBase
    {
        private ICachebaleDocumentTypeRepository _documentTypeRepository { get; set; }
        private ICachebaleCategoryRepository _categoryRepository { get; set; }

        public DocumentTypeController(ICachebaleDocumentTypeRepository documentTypeRepo, ICachebaleCategoryRepository categoryRepo)
        {
            _documentTypeRepository = documentTypeRepo;
            _categoryRepository = categoryRepo;
        }
        /// <summary>
        /// Creates or updates Document Types.
        /// </summary>
        /// <param name="documentTypesToCreate">Array of document Types to Create.</param>
        /// <returns>The created document Type</returns>
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(DocumentTypeSimpleDtoListResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        [HttpPost]
        [SwaggerOperation(
            Description = "Requires admin privileges",
            OperationId = "CreateOrUpdateDocumentTypes",
            Tags = new[] { "Document Type" }
        )]
        public async Task<ActionResult<DocumentTypeSimpleDtoListResult>> CreateOrUpdateDocumentTypes([FromBody] DocumentTypeToCreateDto[] documentTypesToCreate)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var result = _documentTypeRepository.CreateOrUpdateEntities(documentTypesToCreate);
                return Ok(new DocumentTypeSimpleDtoListResult());//result.Convert
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode(StatusCodes.Status500InternalServerError, ex.Message));
            }
        }

        ///// <summary>
        ///// Gets all documentTypes.
        ///// </summary>
        ///// <returns>List of All document Types</returns>
        //[HttpGet()]
        //[SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<DocumentTypeDto>))]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        //[ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        //[ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        //[SwaggerOperation(
        //    Description = "Requires admin privileges",
        //    OperationId = "GetAllDocumentTypes",
        //    Tags = new[] { "Document Type" }
        //)]
        //public async Task<ActionResult<ResultObj<IEnumerable<DocumentTypeDto>>>> GetAll()
        //{
        //    try
        //    {
        //        return Ok(_documentTypeRepository.FindMany());
        //    }
        //    catch (Exception ex)
        //    {
        //        return await Task.FromResult(StatusCode(StatusCodes.Status500InternalServerError, ex.Message));
        //    }
        //}
        /// <summary>
        /// Gets a document type.
        /// </summary>
        /// <param name="docTypeCode">The Key of the document Type to look for.</param>
        /// <returns>The document Type if found.</returns>
        [HttpGet("{docTypeCode}")]
        [ProducesResponseType(typeof(DocumentTypeDtoResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        [SwaggerOperation(
            Description = "Requires admin privileges",
            OperationId = "GetDocumentType",
            Tags = new[] { "Document Type" }
        )]
        public async Task<ActionResult<DocumentTypeDtoResult>> GetDocumentType(string docTypeCode)
        {
            try
            {
                var documentType = _documentTypeRepository.FindByKey( doc=>doc.DocTypeCode== docTypeCode);
                if (documentType == null)
                {
                    return new ConflictObjectResult("The document Type does not exist");
                }
                return Ok(new DocumentTypeDtoResult());
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode(StatusCodes.Status500InternalServerError, ex.Message));
            }
        }

        /// <summary>
        /// Gets document types of a category.
        /// </summary>
        /// <param name="categoryCode">Category Code to which document types are attached.</param>
        /// <returns>Document Types associated to the specified category.</returns>
        [HttpGet("{categoryCode}")]
        [ProducesResponseType(typeof(DocumentTypeDtoResult),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Description = "Requires admin privileges",
            OperationId = "GetDocumentTypeByCategory",
            Tags = new[] { "Document Type" }
        )]
        public async Task<ActionResult<DocumentTypeDtoResult>> GetDocumentTypeByCategory(string categoryCode)
        {
            try
            {
                if (!_categoryRepository.FindByKey(cat=>cat.Code==categoryCode).Success)
                {
                    return new ConflictObjectResult("Category does not exist");
                }
                return Ok(new DocumentTypeDtoResult());//_documentTypeRepository.FindMany(doc=>doc.Category.Code==categoryCode).ConvertoTo
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode(StatusCodes.Status500InternalServerError, ex.Message));
            }
        }

        /// <summary>
        /// Removes a document type.
        /// </summary>
        /// <param name="docTypeCode">Document Type code to remove</param>
        /// <returns>Ok if successfully removed</returns>
        [HttpDelete("{docTypeCode}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest,Type=typeof(DocumentTypeSimpleDto))]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Description = "Requires admin privileges",
            OperationId = "RemoveDocumentType",
            Tags = new[] { "Document Type" }
        )]
        public async Task<IActionResult> RemoveDocumentType(string docTypeCode)
        {
            try
            {
                var documentType = _documentTypeRepository.Remove(doc=>doc.DocTypeCode==docTypeCode);
                if (documentType == null)
                {
                    return new ConflictObjectResult("Document Type to delete does not exist");
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