using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using WebApiAuth.Models;
using WebApiAuth.Models.Dto;

namespace WebApiAuth.Authorization
{
    public class TokenHelper
    {

        public static string GenerateToken(User user, List<Role> roles, List<PermissionDto> permissions, ApplicationSettings appSettings)
        {
            var claims = new []
             {
                 new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                 new Claim(ClaimTypes.Email, user.Email),
                 new Claim(ClaimTypes.Name, user.Name)
             }.Union(
                permissions.Select(
                    x => new Claim(x.PermissionType, x.PermissionValue)).ToList()
             ).Union(
                roles.Select(x => new Claim(ClaimTypes.Role, x.Name)).ToList()
                );

            var claimsIdentity = new ClaimsIdentity(claims);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Convert.FromBase64String(appSettings.SecretKey);
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Claims = claims.GroupBy(x=>x.Type).Select(x=>x.FirstOrDefault()).ToDictionary(x=>x.Type,x=>(object)x.Value),
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
