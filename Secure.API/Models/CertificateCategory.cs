namespace Secure.API.Models;

public class CertificateCategory
{
    public string? Issuer { get; set; }
    public string? PublicKey { get; set; }
    public string? PrivateKey { get; set; }
    public string? Jwks { get; set; }
    public DateTime ExpireTime { get; set; }
    public DateTime IssuedAt { get; set; }
}