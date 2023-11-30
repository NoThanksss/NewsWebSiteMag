using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using NewsWebSite_BLL.Interfaces;
using NewsWebSite_BLL.Models;
using NewsWebSite_DAL.Models;
using System.IdentityModel.Tokens.Jwt;

namespace NewsWebSite_BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AccountDB> _userManager;
        private readonly JwtHandler _jwtHandler;
        private readonly IUserService _userService;

        public AuthService(UserManager<AccountDB> userManager, JwtHandler jwtHandler,
            IUserService userService)
        {
            _userManager = userManager;
            _jwtHandler = jwtHandler;
            _userService = userService;
        }

        public async Task<AuthResponse> LoginAsync(LoginModel model)
        {
            AccountDB user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return new AuthResponse { ErrorMessage = "Invalid Authentication" };
            }

            string token = await GenerateTokenAsync(user);
            return new AuthResponse { IsAuthSuccessful = true, Token = token };
        }

        public async Task<AuthResponse> RegisterAsync(RegisterModel model)
        {
            User profile = new User()
            {
                BirthDate = model.BirthDate,
                FullName = model.FullName,
                Gender = model.Gender,
                PhoneNumber = model.PhoneNumber,

            };
            var addedProfile = _userService.AddNewUser(profile);
            AccountDB user = new AccountDB()
            {
                FirstName = addedProfile.FullName,
                LastName = addedProfile.FullName,
                UserName = model.Email,
                UserDBId = addedProfile.Id,
                Email = model.Email,

            };
            var createdUser = await _userManager.CreateAsync(user, model.Password);
            if (createdUser.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
                string token = await GenerateTokenAsync(user);
                return new AuthResponse { IsAuthSuccessful = true, Token = token };
            }
            return new AuthResponse { IsAuthSuccessful = false, ErrorMessage = createdUser.Errors.First().Description };

            
        }

        private async Task<string> GenerateTokenAsync(AccountDB user)
        {
            Microsoft.IdentityModel.Tokens.SigningCredentials signingCredentials = _jwtHandler.GetSigningCredentials();
            List<Claim> claims = _jwtHandler.GetClaims(user);
            var roles = await _userManager.GetRolesAsync(user);
            AddRolesToClaims(claims, roles);
            JwtSecurityToken tokenOptions = _jwtHandler.GenerateTokenOptions(signingCredentials, claims);
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }
        private void AddRolesToClaims(List<Claim> claims, IEnumerable<string> roles)
        {
            foreach (var role in roles)
            {
                var roleClaim = new Claim(ClaimTypes.Role, role);
                claims.Add(roleClaim);
            }
        }
    }
}
