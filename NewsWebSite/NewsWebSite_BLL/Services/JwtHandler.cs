using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NewsWebSite_DAL.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NewsWebSite_BLL.Services
{
    public class JwtHandler
    {
        private readonly IConfiguration _configuration;
        private readonly IConfigurationSection _jwtSettings;
        public JwtHandler(IConfiguration configuration)
        {
            _configuration = configuration;
            _jwtSettings = _configuration.GetSection("JwtSettings");
        }
        public SigningCredentials GetSigningCredentials()
        {
            byte[] key = Encoding.UTF8.GetBytes(_jwtSettings.GetSection("securityKey").Value);
            SymmetricSecurityKey secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }
        public List<Claim> GetClaims(AccountDB account)
        {
            List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, account.Email)
        };
            return claims;
        }
        public JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            JwtSecurityToken tokenOptions = new JwtSecurityToken(
                issuer: _jwtSettings.GetSection("validIssuer").Value,
                audience: _jwtSettings.GetSection("validAudience").Value,
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtSettings.GetSection("expiryInMinutes").Value)),
                signingCredentials: signingCredentials);
            return tokenOptions;
        }
    }
}
