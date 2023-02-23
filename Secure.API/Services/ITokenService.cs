using Secure.API.Dtos;

namespace Secure.API.Services;

public interface ITokenService
{
    Task<string> GetJweToken();

    Task<JwksResponse> GetJwks();
}