using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ObjectClasses;

namespace Application.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfigurationSection _jwtSettings;

        Context _context;
        public LoginController(IConfiguration configuration, Context context)
        {

            _jwtSettings = configuration.GetSection("JwtSettings");
            _context = context;
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<ResultViewModel> Login(LoginViewModel model)
        {
            var user = _context.Employees.Where(s=>s.UserName.Equals(model.UserName)  && s.Password.Equals(model.Password)).Any();

            if (user)
            {
                var tmp = _context.Employees.Where(s => s.UserName.Equals(model.UserName) && s.Password.Equals(model.Password)).FirstOrDefault();
                var signingCredentials = GetSigningCredentials();
                var claims = GetClaims(tmp);
                var tokenOptions = GenerateTokenOptions(signingCredentials, await claims);
                var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                tmp.IsEligible = true;
                tmp.IsExpired = false;
                _context.SaveChanges();

                //HttpContext.Session.SetString("SessionName", token);

                
                return new ResultViewModel { IsSuccess = true, Data = token, IdOfUser = tmp.Id };
            }
            return new ResultViewModel { IsSuccess = false, Message = "Invalid username or password.", };



        }

        [AllowAnonymous]
        private SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(_jwtSettings.GetSection("securityKey").Value);
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        [AllowAnonymous]
        private async Task<List<Claim>> GetClaims(Employee user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                //new Claim(ClaimTypes.MobilePhone, user.PhoneNumber)
            };
           /* var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }*/
            return claims;
        }

        [AllowAnonymous]
        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var tokenOptions = new JwtSecurityToken(
                issuer: _jwtSettings.GetSection("validIssuer").Value,
                audience: _jwtSettings.GetSection("validAudience").Value,
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtSettings.GetSection("expiryInMinutes").Value)),
                signingCredentials: signingCredentials);
            return tokenOptions;
        }

    }
}

