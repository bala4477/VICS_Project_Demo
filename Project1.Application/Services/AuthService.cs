using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Project1.Application.Input_Models;
using Project1.Application.Services.Interface;
using Project1.Application.View_Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Project1.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;
        private IdentityUser IdentityUser;

        public AuthService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
            IdentityUser = new();
            _signInManager = signInManager;
        }

        public async Task<object> Login(Login login)
        {
            IdentityUser = await _userManager.FindByEmailAsync(login.Email);
            if (IdentityUser == null)
            {
                return "Invalid Email Address";
            }

            var result = await _signInManager.PasswordSignInAsync(IdentityUser, login.Password, isPersistent: true, lockoutOnFailure: true);

            var isValidCredential = await _userManager.CheckPasswordAsync(IdentityUser, login.Password);

            if (result.Succeeded)
            {
                var token = await GenerateToken();

                LoginResponse loginResponse = new LoginResponse
                {
                    UserId = IdentityUser.Id,
                    Token = token,
                };
                return loginResponse;
            }
            else
            {
                if (result.IsLockedOut)
                {
                    return "Your Account Locked, Contact System Admin";
                }
                if (result.IsNotAllowed)
                {
                    return "Verify Email Address";
                }
                if (isValidCredential == false)
                {
                    return "Invalid Password";
                }
                else
                {
                    return "Login Failed";
                }
            }
        }

        public async Task<IEnumerable<IdentityError>> Register(Register register)
        {
            IdentityUser.UserName = register.Email;
            IdentityUser.Email = register.Email;

            var result = await _userManager.CreateAsync(IdentityUser);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(IdentityUser, "ADMIN");
            }

            return result.Errors;
        }

        public async Task<string> GenerateToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));

            var signingCredentials = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);

            var roles = await _userManager.GetRolesAsync(IdentityUser);

            var roleClaims = roles.Select(x=> new Claim(ClaimTypes.Role,x)).ToList();

            List<Claim> claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email,IdentityUser.Email)
            }.Union(roleClaims).ToList();

            var token = new JwtSecurityToken
                (
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["JwtSettings:DurationInMinutes"]))
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
