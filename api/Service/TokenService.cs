using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api.Interfaces;
using api.Models;
using Microsoft.IdentityModel.Tokens;

namespace api.Service;

public class TokenService : ITokenService {
    private readonly SymmetricSecurityKey _key;
    private readonly IConfiguration _config;

    public TokenService(IConfiguration config) {
        _config = config;
        // This key is used to sign (aka, create) and/or validate JWTs
        // First, convert the SigninKey (string) to a byte[] using UTF-8 encoding
        // Cryptographic algorithms (HMAC-SHA512) work with raw bytes, not strings
        // Then, create a Symmetric Key using the byte values of each character
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:SigningKey"]));
    }
    
    public string CreateToken(AppUser user) {
        var claims = new List<Claim> {
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Name, user.UserName)
        };

        var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = creds,
            Issuer = _config["JWT:Issuer"],
            Audience = _config["JWT:Audience"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}