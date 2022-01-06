using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Authorization
{
    public class TokenHelper
    {

        public static string GenerateToken(ApplicationUser user, List<string> roles, List<Claim> claims, ApplicationSettings appSettings)
        {
            IdentityOptions _options = new IdentityOptions();
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Convert.FromBase64String(appSettings.SecretKey);
            var claimsIdentity = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Name, user.UserName.ToString()),
                new Claim(ClaimTypes.Email,user.Email)
            });

            foreach (var item in roles.GroupBy(x=>x).Select(x=>x.FirstOrDefault()))
            {
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, item));
            }
            foreach (var item in claims.GroupBy(x => x).Select(x => x.FirstOrDefault()))
            {
                claimsIdentity.AddClaim(new Claim(item.Type, item.Value));
            }
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Issuer = appSettings.Issuer,
                Audience = appSettings.Audience,
                Expires = DateTime.Now.AddMinutes(15),
                SigningCredentials = signingCredentials,
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

}
