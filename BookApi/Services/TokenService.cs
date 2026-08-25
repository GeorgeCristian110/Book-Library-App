using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookApi.Models;
using Microsoft.IdentityModel.Tokens;

namespace BookApi.Services;

public class TokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
          new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
          new Claim(JwtRegisteredClaimNames.Email, user.Email),
          new Claim(ClaimTypes.Role, user.Role)
        };

        var keyBytes = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);
        var signingKey = new SymmetricSecurityKey(keyBytes);
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken
        (
            issuer : _configuration["Jwt:Issuer"],
            audience : _configuration["Jwt:Audience"],
            claims : claims,
            expires : DateTime.UtcNow.AddHours(1),
            signingCredentials : credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}