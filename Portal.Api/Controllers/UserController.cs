using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using AutoMapper;
using System.Linq;
using Assette.Client;
using Framework.Common;
using SieveModel = Sieve.Models.SieveModel;
using Portal.Api.Repositories;
using Portal.Api.Repositories.Contracts;

namespace Portal.Api.Controllers
{
    /// <summary>
    /// Manages users
    /// </summary>
    [Route("api/v{version:apiVersion}/users")]
    [Produces("application/json", "application/xml")]
    [Consumes("application/json", "application/xml")]
   // [Authorize]
    [ApiController]
        public class UserController : ControllerBase
    {
        private ICachebaleUserRepository _userRepository { get; }
        private ICachebaleAssociationRepository _associationRepository { get; }
        private IMapper _mapper { get; }

        public UserController(ICachebaleUserRepository userRepository,
            ICachebaleAssociationRepository associationRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _associationRepository = associationRepository;
            _mapper = mapper;
        }
        #region CRUD
        /// <summary>
        /// Creates or updates users.
        /// </summary>
        /// <param name="usersToCreate">Array of users to create</param>
        /// <returns>List of Results</returns>
        [HttpPost()]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "List of results for each created user.",Type=typeof(UserSimpleDtoListResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(IEnumerable<UserSimpleDtoResult>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Description = "Requires authentication",
            OperationId = "CreateOrUpdateUsers",
            Tags = new[] { "User" }
        )]
        public async Task<ActionResult<UserSimpleDtoListResult>> CreateOrUpdateUsers([FromBody] UserToCreateDto[] usersToCreate)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var results = _userRepository.CreateOrUpdateEntities(usersToCreate, "UserCode", "UserCode");
                var assetteResult = results.ToList().ConvertTo();//results.Convert
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
        ///// <summary>
        ///// Updates users in bulk.
        ///// </summary>
        ///// <param name="usersToUpdate">Array of users to create</param>
        ///// <returns>List of Results</returns>
        //[HttpPost("bulk/update")]
        //[SwaggerResponse(StatusCodes.Status200OK, Description = "List of results for each updated user.", Type = typeof(IEnumerable<UserSimpleDtoResult>))]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        //[ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        //[SwaggerOperation(
        //    Description = "Requires authentication",
        //    OperationId = "UpdateBulkUsers",
        //    Tags = new[] { "User" }
        //)]
        //public async Task<ActionResult<IEnumerable<UserSimpleDtoResult>>> Update([FromBody] UserDto[] usersToUpdate)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(ModelState);
        //        }
        //        var results = _userRepository.BulkUpdate(usersToUpdate);
        //        return Ok(results);
        //    }
        //    catch (Exception ex)
        //    {
        //        return await Task.FromResult(StatusCode(StatusCodes.Status500InternalServerError, ex.Message));
        //    }
        //}

        /// <summary>
        /// Deactivates a user.
        /// </summary>
        /// <param name="userCode">The key of the user to deactivate.</param>
        /// <returns>Ok if successfully Activated the account</returns>
        [HttpPatch("deactivate/{userCode}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserSimpleDtoResult))]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Description = "Requires authentication",
            OperationId = "DeactivateUser",
            Tags = new[] { "User" }
        )]
        public async Task<ActionResult<UserSimpleDtoResult>> Deactivate(string userCode)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(userCode))
                {
                    return BadRequest();
                }
                var resultFind = _userRepository.FindByKey(user => user.UserCode == userCode);
                if (!resultFind.Success)//user already exists
                {
                    return new ConflictObjectResult(resultFind.Messages.FirstOrDefault());
                }
                //What if a user is already active->conflict or continue ?
                //var patchedUser = new AccountToCreateDto();//the recently updated account
                var result = _userRepository.Activate(userCode, false);
                var assetteResult = result.ConvertToSimpleDto();
                return Ok(assetteResult);
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode(StatusCodes.Status500InternalServerError, ex.Message));
            }
        }

        /// <summary>
        /// Activates a user
        /// </summary>
        /// <param name="userCode">The key of the user to activate.</param>
        /// <returns>Ok if successfully Activated the account.</returns>
        [HttpPatch("activate/{userCode}")]
        [ProducesResponseType(StatusCodes.Status200OK,Type =typeof(UserSimpleDtoResult))]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Description = "Requires admin privileges",
            OperationId = "ActivateUser",
            Tags = new[] { "User" }
        )]
        public async Task<ActionResult<UserSimpleDtoResult>> Activate(string userCode)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(userCode))
                {
                    return BadRequest();
                }
                var resultFind = _userRepository.FindByKey(user => user.UserCode == userCode);
                if (!resultFind.Success)//user already exists
                {
                    return new ConflictObjectResult(resultFind.Messages.FirstOrDefault());
                }
                //What if a user is already active->conflict or continue ?
               //var patchedUser = new AccountToCreateDto();//the recently updated account
                var result=_userRepository.Activate(userCode, true);
                var assetteResult = result.ConvertToSimpleDto();
                return Ok(assetteResult);
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode(StatusCodes.Status500InternalServerError, ex.Message));
            }
        }

        #endregion


        #region Password & Login

        /// <summary>
        /// Locks a user.
        /// </summary>
        /// <param name="userCode">The user to lock</param>
        /// <returns>200 if user was successfully locked.</returns>
        [HttpPatch("lock/{userCode}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserSimpleDtoResult))]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Description = "Requires admin privileges",
            OperationId = "Lock",
            Tags = new[] { "User" }
        )]
        public async Task<ActionResult<UserSimpleDtoResult>> Lock(string userCode)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(userCode))
                {
                    return BadRequest();
                }
                var resultFind = _userRepository.FindByKey(u => u.UserCode == userCode);
                if (!resultFind.Success)//user already exists
                {
                    return new ConflictObjectResult(resultFind.Messages.FirstOrDefault());
                }
                var user=_userRepository.Lock(userCode, true);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode(StatusCodes.Status500InternalServerError, ex.Message));
            }
        }
        /// <summary>
        /// Unlocks a user
        /// </summary>
        /// <param name="userCode">The user to unlock</param>
        /// <returns>200 if user was successfully unlocked</returns>
        [HttpPatch("unlock/{userCode}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserSimpleDtoResult))]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Description = "Requires admin privileges",
            OperationId = "Unlock",
            Tags = new[] { "User" }
        )]
        public async Task<ActionResult<UserSimpleDtoResult>> Unlock(string userCode)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(userCode))
                {
                    return BadRequest();
                }
                var resultFind = _userRepository.FindByKey(u => u.UserCode == userCode);
                if (!resultFind.Success)//user already exists
                {
                    return new ConflictObjectResult(resultFind.Messages.FirstOrDefault());
                }
                var user=_userRepository.Lock(userCode, false);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode(StatusCodes.Status500InternalServerError, ex.Message));
            }
        }
        /// <summary>
        /// Resets the user password
        /// </summary>
        /// <param name="resetPasswordModel">The reset password Model</param>
        /// <returns>200 if successfully unlocked</returns>
        [HttpPost("resetPassword")]
        [ProducesResponseType(StatusCodes.Status200OK,Type =typeof(Result))]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Description = "Requires admin privileges",
            OperationId = "Resetpassword",
            Tags = new[] { "User" }
        )]
        public async Task<ActionResult<Result>> ResetPassword(ResetPasswordDto resetPasswordModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = _userRepository.FindByKey(user => user.Email == resetPasswordModel.Email);
                if (!result.Success)
                {
                    return new ConflictObjectResult("User to reset password for does not exist");
                }
                var result2 = _userRepository.ResetPassword(resetPasswordModel);
                return Ok(result2);
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode(StatusCodes.Status500InternalServerError, ex.Message));
            }
        }

        /// <summary>
        /// Forgot password.
        /// </summary>
        /// <param name="forgotPasswordModel">Forgot password model</param>
        /// <returns>200 if successfully unlocked</returns>
        [HttpPost("forgotPassword")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result))]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Description = "Requires admin privileges",
            OperationId = "ForgotPassword",
            Tags = new[] { "User" }
        )]
        public async Task<ActionResult<Result>> ForgotPassword(ForgotPasswordDto forgotPasswordModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = _userRepository.FindByKey(user => user.Email == forgotPasswordModel.Email);
                if (!result.Success)
                {
                    return new ConflictObjectResult("User to reset password for does not exist");
                }
                var res2 = _userRepository.ForgotPassword(forgotPasswordModel);
                
                return Ok(res2);
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode(StatusCodes.Status500InternalServerError, ex.Message));
            }
        }
        #endregion
        #region Projections
        /// <summary>
        /// Gets a specific user by key.
        /// </summary>
        /// <param name="userCode">The user key to filter by.</param>
        /// <returns>The user object if found.</returns>
        [HttpGet("{userCode}")]
        [ProducesResponseType(StatusCodes.Status200OK,Type=typeof(UserDtoResult))]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Description = " Requires authentication.",
            OperationId = "GetUser",
            Tags = new[] { "User" }
        )]
        public async Task<ActionResult<UserDtoResult>> GetUser(string userCode)
        {
            try
            {
                var resultFind = _userRepository.FindByKey(u=>u.UserCode==userCode);
                if (!resultFind.Success)
                {
                    return NotFound();
                }
                var AssetteResult = resultFind.ConvertTo();
                return Ok(AssetteResult);
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode(StatusCodes.Status500InternalServerError, ex.Message));
            }
        }

        /// <summary>
        /// Gets accounts for a specific user.
        /// </summary>
        /// <param name="userCode">User key to filter by</param>
        /// <returns>List of accounts</returns>
        [Route("{userCode}/accounts")]
        [HttpGet()]
        [ProducesResponseType(typeof(AccountDtoListResult),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Description = "Requires admin privileges",
            OperationId = "GetAccountsForSpecificUser",
            Tags = new[] { "User" }
        )]
        public async Task<ActionResult<AccountDtoListResult>> GetAccountsForSpecificUser(string userCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userCode))
                {
                    return BadRequest();
                }
                var resultFind = _userRepository.FindByKey(u => u.UserCode == userCode);
                if (!resultFind.Success)//user already exists
                {
                    return new ConflictObjectResult(resultFind.Messages.FirstOrDefault());
                }
                var accounts = _associationRepository.FindMany(acc=>acc.UserCode== userCode);

                return Ok(new AccountDtoListResult());//accounts.convert
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode(StatusCodes.Status500InternalServerError, ex.Message));
            }
        }

        /// <summary>
        /// Gets documentTypes for a specific user
        /// </summary>
        /// <param name="userCode">User key to filter by</param>
        /// <returns>List of accounts</returns>
        [Route("{userCode}/documentTypes")]
        [HttpGet()]
        [ProducesResponseType(typeof(DocumentTypeDtoListResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Description = "Requires admin privileges",
            OperationId = "GetDocumentTypesForSpecificUser",
            Tags = new[] { "User" }
        )]

        public async Task<ActionResult<DocumentTypeDtoListResult>> GetDocumentTypesForSpecificUser(string userCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userCode))
                {
                    return BadRequest();
                }
                var resultFind = _userRepository.FindByKey(u => u.UserCode == userCode);
                if (!resultFind.Success)//user already exists
                {
                    return new ConflictObjectResult(resultFind.Messages.FirstOrDefault());
                }
                var documentTypeResult = _associationRepository.FindMany(docs=>docs.UserCode== userCode);
                return Ok(new DocumentTypeDtoListResult());//documentTypeResult.convert
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode(StatusCodes.Status500InternalServerError, ex.Message));
            }
        }

        /// <summary>
        /// Gets users by filter.
        /// </summary>
        /// <param name="searchFilter">Generic search model</param>
        /// <returns>The user object if found.</returns>
        [HttpPost("getUsersByFilter")]
        [Route("{userCode}", Name = "GetUserByFilter")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDtoListResult))]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Description = " Requires authentication.",
            OperationId = "SearchUsers",
            Tags = new[] { "User" }
        )]
        public async Task<ActionResult<UserDtoListResult>> SearchUsers(SieveModel searchFilter)
        {
            try
            {
                var resultFind = _userRepository.SearchFor(searchFilter);
                if (!resultFind.Success)//user already exists
                {
                    return new ConflictObjectResult(resultFind.Messages.FirstOrDefault());
                }
               var assetteResult= resultFind.ConvertTo();

                return Ok(assetteResult);
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode(StatusCodes.Status500InternalServerError, ex.Message));
            }
        }
        #endregion
    }
}