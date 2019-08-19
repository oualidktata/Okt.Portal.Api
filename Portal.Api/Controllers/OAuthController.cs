using System;
using System.Threading.Tasks;
using AuthenticationServiceProxy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Portal.Api.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json", "application/xml", "text/plain")]
    [Consumes("application/json", "application/xml", "text/plain")]
    [ApiController]
    public class OAuthController : ControllerBase
    {
        private ITokenService _tokenService { get; set; }
        public OAuthController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        [SwaggerOperation(
           Description = "Authenticate the API",
           OperationId = "GetAccessToken",
           Tags = new[] { "*GetToken 1st*" })]
        [HttpPost("token")]
        [AllowAnonymous]
        public async Task<IActionResult> Token()
        {
                try
                {
                    var token = await _tokenService.GetToken();
                    if (token == null)
                    {
                    return await Task.FromResult(StatusCode(StatusCodes.Status500InternalServerError, "Token is empty"));
                }
                return Ok(token);
                }
                catch (Exception ex)
                {
                    return await Task.FromResult(StatusCode(StatusCodes.Status500InternalServerError, ex.Message));
                }
        }
    }
}