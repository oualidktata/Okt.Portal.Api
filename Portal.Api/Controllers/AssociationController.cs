using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Assette.Client;
using SieveModel = Sieve.Models.SieveModel;
using Portal.Api.Repositories.Contracts;

namespace Portal.Api.Controllers
{
    /// <summary>
    /// Manage associations (user-account-documentType)
    /// </summary>
    [Route("api/v{version:apiVersion}/doctypeassociations")]
    [Produces("application/json", "application/xml")]
    [Consumes("application/json", "application/xml")]
    [ApiController]
    public class DocTypeAssociationController : ControllerBase
    {
        private ICachebaleAssociationRepository _associationRepository { get; }
        private ICachebaleUserRepository _userRepository { get; }

        public DocTypeAssociationController(ICachebaleAssociationRepository associationRepository,
                                    ICachebaleUserRepository userRepository)
        {
            _associationRepository = associationRepository;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Creates Associations.
        /// </summary>
        /// <param name="associationsToCreate">Array of associations to create</param>
        /// <returns>List of Results</returns>
        [HttpPost()]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "List of results for each created association.", Type = typeof(AssociationSimpleDtoListResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(string), StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Description = "Requires authentication",
            OperationId = "CreateDocumentTypeAssociations",
            Tags = new[] { "Document Type Associations" }
        )]
        public async Task<ActionResult<AssociationSimpleDtoListResult>> CreateDocumentTypeAssociations([FromBody] AssociationDto[] associationsToCreate)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var results = _associationRepository.CreateOrUpdateEntities(associationsToCreate);

                return Ok(new AssociationSimpleDtoListResult());//ConvertResult
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode(StatusCodes.Status500InternalServerError, ex.Message));
            }
        }
        /// <summary>
        /// Delete a specific Association.
        /// </summary>
        /// <param name="userCode"></param>
        /// <param name="accountCode"></param>
        /// <param name="documentTypeCode"></param>
        /// <returns>List of Results</returns>
        [HttpDelete()]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(AssociationSimpleDto))]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Description = "Requires authentication",
            OperationId = "RemoveDocumentTypeAssociation",
            Tags = new[] { "Document Type Associations" }
        )]
        public async Task<IActionResult> RemoveDocumentTypeAssociation(string userCode, string accountCode, string documentTypeCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userCode) || string.IsNullOrWhiteSpace(accountCode) || string.IsNullOrWhiteSpace(documentTypeCode))
                {
                    return BadRequest(new AssociationSimpleDto() { UserCode = userCode, AccountCode = accountCode, DocumentTypeCode = documentTypeCode });
                }
                var results = _associationRepository.RemoveAssociation(userCode, accountCode, documentTypeCode);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode(StatusCodes.Status500InternalServerError, ex.Message));
            }
        }

        /// <summary>
        /// Removes Associations in bulk.
        /// </summary>
        /// <param name="body">Array of associations to remove</param>
        /// <returns>List of Results</returns>
        [HttpPost("bulk/delete")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "List of results for each removed association.", Type = typeof(AssociationSimpleDtoListResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Description = "Requires authentication",
            OperationId = "RemoveDocumentTypeAssociations",
            Tags = new[] { "Document Type Associations" }
        )]
        public async Task<ActionResult<AssociationSimpleDtoListResult>> RemoveDocumentTypeAssociations([FromBody] AssociationDto[] body)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var results = _associationRepository.RemoveAssociations(body);
                return Ok(new AssociationSimpleDtoListResult());//results.convert
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode(StatusCodes.Status500InternalServerError, ex.Message));
            }
        }


       
        ///// <summary>
        ///// Gets a specific association.
        ///// </summary>
        ///// <param name="userCode">The user key associated to the association</param>
        ///// <param name="accountCode">The account associated to the association</param>
        ///// <param name="documentTypeCode">The document Type associated to the association</param>
        ///// <returns>The association object</returns>
        //[HttpGet()]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultObj<AssociationDto>))]
        //[ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        //[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        //[ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        //[SwaggerOperation(
        //    Description = "Requires authentication",
        //    OperationId = "GetAssociation",
        //    Tags = new[] { "Document Type Associations" }
        //)]
        //public async Task<ActionResult<ResultObj<AssociationDto>>> GetAssociation(string userCode, string accountCode, string documentTypeCode)
        //{
        //    try
        //    {
        //        if (string.IsNullOrWhiteSpace(userCode) || string.IsNullOrWhiteSpace(accountCode) || string.IsNullOrWhiteSpace(documentTypeCode))
        //        {
        //            return BadRequest(ModelState);
        //        }

        //        var associations = _associationRepository.FindBy(userCode, accountCode, documentTypeCode).Data.ToList();

        //        if (!associations.Any())
        //        {
        //            return NotFound();
        //        }
        //        if (associations.Count > 1)
        //        {
        //            return new ConflictObjectResult($"More then one association were found for User-Account-DocumentType:{userCode}-{accountCode}-{documentTypeCode}");
        //        }
        //        return Ok(new Result<AssociationSimpleDto>(true, associations.FirstOrDefault()));
        //    }
        //    catch (Exception ex)
        //    {
        //        return await Task.FromResult(StatusCode(StatusCodes.Status500InternalServerError, ex.Message));
        //    }
        //}

        ///// <summary>
        ///// Gets associations of a specific user.
        ///// </summary>
        ///// <param name="userCode">The user key associated to the association</param>
        ///// <returns>List of associations.</returns>
        //[HttpGet("associations")]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultObj<IEnumerable<AssociationDto>>))]
        //[ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        //[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        //[ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        //[SwaggerOperation(
        //    Description = "Requires authentication",
        //    OperationId = "GetUserassociations",
        //    Tags = new[] { "Document Type Associations" }
        //)]
        //public async Task<ActionResult<ResultObj<IEnumerable<AssociationDto>>>> GetAssociations(string userCode)
        //{
        //    try
        //    {
        //        if (string.IsNullOrWhiteSpace(userCode))
        //        {
        //            return BadRequest(ModelState);
        //        }

        //        var user = _userRepository.FindByKey(u=>u.UserCode==userCode);
        //        if (user == null)
        //        {
        //            return new ConflictObjectResult("The user does not exist");
        //        }

        //        var associationsResult = _associationRepository.FindMany(a=>a.UserCode==userCode);

        //        if (!associationsResult.Data.Any())
        //        {
        //            return NotFound();
        //        }
                
        //        return Ok(associationsResult);
        //    }
        //    catch (Exception ex)
        //    {
        //        return await Task.FromResult(StatusCode(StatusCodes.Status500InternalServerError, ex.Message));
        //    }
        //}

        /// <summary>
        /// Gets associations by filter.
        /// </summary>
        /// <param name="searchModel">Generic search model</param>
        /// <returns>List of associations.</returns>
        [HttpPost("search")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AssociationDtoListResult))]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Description = "Requires authentication",
            OperationId = "SearchDocumentTypeAssociations",
            Tags = new[] { "Document Type Associations" }
        )]
        public async Task<ActionResult<AssociationDtoListResult>> SearchDocumentTypeAssociations([FromBody]SieveModel searchModel)
        {
            try
            {
                 if (searchModel==null)
                {
                    return BadRequest(ModelState);
                }
                var associations = _associationRepository.SearchFor(searchModel);

                if (!associations.Data.Any())
                {
                    return NotFound();
                }
                
                return Ok(new AssociationDtoListResult()
                {
                    Success = associations.Success,
                    Data = associations.Data.ToList()
                });
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode(StatusCodes.Status500InternalServerError, ex.Message));
            }
        }
    }
}