using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Portal.Api.Repositories.Contracts;
using Assette.Client;
using System.Linq;
using Portal.Api.Repositories;

using SieveModel = Sieve.Models.SieveModel;
using Portal.Api.Repositories.Repos;

namespace Portal.Api.Controllers
{
    [Route("api/v{version:apiVersion}/accounts")]
    [Produces("application/json", "application/xml")]
    [Consumes("application/json", "application/xml")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private ICachebaleAccountRepository _accountRepository { get; }
        private ICachebaleUserRepository _userRepository { get; }


        public AccountController(ICachebaleAccountRepository accountRepository,
            ICachebaleUserRepository userRepository)
        {
            _accountRepository = accountRepository;
            _userRepository = userRepository;
        }

        
        /// <summary>
        /// Creates or updates one or more accounts.
        /// </summary>
        /// <param name="accountsToCreate">Array of accounts to create</param>
        /// <returns>List of Results</returns>
        [HttpPost()]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "List of results for each upserted account.", Type = typeof(AccountSimpleDtoListResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Description = "Requires authentication",
            OperationId = "CreateOrUpdateAccounts",
            Tags = new[] { "Account" }
        )]
        public async Task<ActionResult<AccountSimpleDtoListResult>> CreateOrUpdateAccounts([FromBody] AccountToCreateDto[] accountsToCreate)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var results = _accountRepository.CreateOrUpdateEntities(accountsToCreate,"Code","Code");
                var assetteResult = results.ToList().ConvertTo();
                return Ok(assetteResult);
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode(StatusCodes.Status500InternalServerError, ex.Message));
            }
        }
        /// <summary>
        /// Gets accounts by filter.
        /// </summary>
        /// <returns>List of All available accounts.</returns>
        [HttpPost("search")]
        [ProducesResponseType(typeof(AccountDtoListResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Description = "Requires admin privileges",
            OperationId = "SearchAccounts",
            Tags = new[] { "Account" }
        )]
        public async Task<ActionResult<AccountDtoListResult>> SearchAccounts(SieveModel searchModel)
        {
            try
            {
                var result= _accountRepository.SearchFor(searchModel);
                var assetteResult = result.ConvertTo();
                return Ok(assetteResult);
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode(StatusCodes.Status500InternalServerError, ex.Message));
            }

        }
        /// <summary>
        /// Removes an account.
        /// </summary>
        /// <param name="accountCodes">The code of the account to remove.</param>
        /// <returns>200 if successfully removed</returns>
        [HttpDelete()]
        [ProducesResponseType(typeof(AccountSimpleDtoListResult),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Result))]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Description = "Requires admin privileges",
            OperationId = "RemoveAccounts",
            Tags = new[] { "Account" }
        )]
        public async Task<ActionResult<AccountSimpleDtoListResult>> Remove([FromBody]string[] accountCodes)
        {
            try
            {
                if (accountCodes == null || !accountCodes.Any())
                {
                    return BadRequest(new Result {
                        Success = false,
                        Messages = new string[] { "Empty list of accounts" }
                    });
                }
                var listOfResults = _accountRepository.Remove<string,bool>(accountCodes,e=>e.Code, e=>e.IsActive,"IsActive",new AccountSimpleDto() { AccountCode=string.Empty});
                var assetteResult = listOfResults.ToList().ConvertTo();
                return Ok(assetteResult);
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode(StatusCodes.Status500InternalServerError, ex.Message));
            }
        }

        /// <summary>
        /// Gets a specific account in the system based on the filter.
        /// </summary>
        /// <param name="accountCode">The account code to look for.</param>
        ///<returns>The account to look for.</returns>
        [HttpGet("{accountCode}")]
        [ProducesResponseType(typeof(AccountDtoResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Description = "Requires admin privileges",
            OperationId = "GetAccount",
            Tags = new[] { "Account" }
        )]
        public async Task<ActionResult<AccountDtoResult>> GetAccount(string accountCode)
        {
            try
            {
                var result=_accountRepository.FindByKey(acc=>acc.Code== accountCode);
                if (!result.Success){
                    return NotFound();
                }
                var assetteResult=result.ConvertTo();
                return Ok(assetteResult);
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode(StatusCodes.Status500InternalServerError, ex.Message));
            }
        }
        
    }
}