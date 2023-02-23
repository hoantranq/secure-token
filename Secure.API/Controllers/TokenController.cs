using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Secure.API.Services;

namespace Secure.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TokenController : ControllerBase
{
	private readonly ITokenService _tokenService;

    public TokenController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpGet("jwe-token")]
    public async Task<ActionResult> GetJweToken()
    {
         var jweToken = await _tokenService.GetJweToken();

        return Ok(jweToken);
    }

    [HttpGet("jwks")]
    public async Task<ActionResult> GetJwks()
    {
        var jwks = await _tokenService.GetJwks();

        return Ok(jwks);
    }
}