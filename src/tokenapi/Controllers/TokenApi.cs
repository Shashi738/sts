using Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace tokenapi.Controllers
{
    [ApiController]
    [Route("api/connect")]
    public class TokenApi : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public TokenApi(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> Authenticate(User user)
        {
            var token = await _authenticationService.AuthenticateAndGenerateToken(user);

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized();
            }

            return Ok(token);
        }

        [Authorize]
        [HttpGet("validate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> ValidateTokenAsync()
        {
            var userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if(string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(userRole))
            {
                return Unauthorized();
            }

            return Ok(true);
        }
    }
}
