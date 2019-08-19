using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using Portal.Api.Repositories.Contracts;
using Assette.Client;
using Framework.Common;

namespace Portal.Api.Controllers
{
    [Route("api/v{version:apiVersion}/doctypecategories")]
    [Produces("application/json", "application/xml")]
    [Consumes("application/json", "application/xml")]
    [ApiController]
    public class CategoryController : ControllerBase
    {

        private ICachebaleCategoryRepository _categoryRepo { get; set; }
        public CategoryController(ICachebaleCategoryRepository categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        /// <summary>
        /// Creates or updates categories of document types
        /// </summary>
        /// <param name="categoriesToCreate">Array of Categories to create</param>
        /// <returns>201 if successfully created.</returns>
        [HttpPost]
        [SwaggerResponse(StatusCodes.Status200OK,"List of results for all created categories",Type= typeof(CategorySimpleDtoListResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [ProducesDefaultResponseType]
        [SwaggerOperation(
            Description = "Requires admin privileges",
            OperationId = "CreateOrUpdateCategories",
            Tags = new[] { "Category" }
        )]
        public async Task<ActionResult<CategorySimpleDtoListResult>> CreateOrUpdateCategories ([FromBody] CategoryToCreateDto[] categoriesToCreate)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var creationResult = _categoryRepo.CreateOrUpdateEntities(categoriesToCreate);

                return Ok(new CategorySimpleDtoListResult());//creationResult.ConvertTo
                //return CreatedAtAction("GetCategory", createdCategory);
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode(StatusCodes.Status500InternalServerError, ex.Message));
            }
        }
        /// <summary>
        /// Get All categories
        /// </summary>
        /// <returns>All categories</returns>
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryDtoListResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Description = "Requires admin privileges",
            OperationId = "GetAllCategoriesAsync",
            Tags = new[] { "Category" }
        )]
        public async Task<ActionResult<CategoryDtoListResult>> GetAllCategories()
        {
            try
            {
                return Ok(new CategoryDtoListResult());//_categoryRepo.FindMany(null).ConvertTo()
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode(StatusCodes.Status500InternalServerError, ex.Message));
            }

        }

        /// <summary>
        /// Gets a specific category.
        /// </summary>
        /// <param name="categoryCode">The Key of the Category to look for.</param>
        /// <returns>Category DTO if found.</returns>
        [HttpGet("{categoryCode}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryDtoResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        [SwaggerOperation(
            Description = "Requires admin privileges",
            OperationId = "GetCategory",
            Tags = new[] { "Category" }
        )]
        public async Task<ActionResult<CategoryDtoResult>> GetCategory(string categoryCode)
        {
            try
            {
                var categoryResult = _categoryRepo.FindByKey( c=>c.Code==categoryCode);
                if (categoryResult == null)
                {
                    return NotFound();
                }
                return Ok(new CategoryDtoResult());//categoryResult
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode(StatusCodes.Status500InternalServerError, ex.Message));
            }
        }
        
        /// <summary>
        /// Remove a category
        /// </summary>
        /// <param name="categoryCode">Category to remove</param>
        /// <returns>Ok if successfully removed</returns>
        [HttpDelete("{categoryCode}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest,Type=typeof(CategoryDtoResult))]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Description = "Requires admin privileges",
            OperationId = "RemoveCategory",
            Tags = new[] { "Category" }
        )]
        public async Task<IActionResult> Remove(string categoryCode)
        {
            try
            {
                var categoryResult = _categoryRepo.Remove(cat=>cat.Code== categoryCode);
                //To do: Handle other return types later
                if (!categoryResult.Success)
                {
                    return BadRequest(new CategoryDtoResult());//categoryResult.convert
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