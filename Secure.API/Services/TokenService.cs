using Jose;
using Secure.API.Dtos;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Secure.API.Services;

public class TokenService : ITokenService
{
    private IWebHostEnvironment _hostingEnvironment;

    public TokenService(IWebHostEnvironment hostingEnvironment)
    {
        _hostingEnvironment = hostingEnvironment;
    }

    public async Task<string> GetJweToken()
    {
        // Get server certificate
        var serverCert = GetServerCertificate();

        // Read private key from certificate
        var certPrivateKey = serverCert.GetRSAPrivateKey();

        // Read public key from certificate
        var certPublicKey = serverCert.GetRSAPublicKey();

        // Payload information
        var payload = new Dictionary<string, string>
        {
            { "iss", "ccdp_token_service" },
            { "sub", "" },
            { "aud", "my_client_id" },
            { "exp", DateTime.Now.AddMilliseconds(36000000).ToString() },
            { "iat", DateTime.Now.ToString() },
        };

        var jwsToken = JWT.Encode(payload, certPrivateKey, JwsAlgorithm.RS256);
        var jweToken = JWT.Encode(jwsToken, certPublicKey, JweAlgorithm.RSA_OAEP_256, JweEncryption.A256CBC_HS512);

        DecodeJweToken(jweToken, certPrivateKey!, certPublicKey!);

        return await Task.FromResult(jweToken);
    }

    public Task<JwksResponse> GetJwks()
    {
        throw new NotImplementedException();
    }

    #region private  helper methods
    private X509Certificate2 GetServerCertificate()
    {
        // Read certificate from pem string
        var certInfo = GetCertificateFilePath();

        X509Certificate2 x509Certificate2cert =
            X509Certificate2.CreateFromPemFile(certInfo.CertPath!, certInfo.CertPrivateKeyPath);

        return x509Certificate2cert;
    }

    private CertInformation GetCertificateFilePath()
    {
        var certPath = Path.Combine(_hostingEnvironment.ContentRootPath, "ServerCertificate\\servercertificate.pem");
        var certPrivateKeyPath = Path.Combine(_hostingEnvironment.ContentRootPath, "ServerCertificate\\privatekey.pem");

        return new CertInformation { CertPath = certPath, CertPrivateKeyPath = certPrivateKeyPath };
    }

    private void DecodeJweToken(string jweToken, RSA certPrivateKey, RSA certPublicKey)
    {
        var jws = JWT.Decode(jweToken, certPrivateKey, JweAlgorithm.RSA_OAEP_256, JweEncryption.A256CBC_HS512);
    }
    #endregion
}

public class CertInformation
{
    public string? CertPath { get; set; }
    public string? CertPrivateKeyPath { get; set; }
}