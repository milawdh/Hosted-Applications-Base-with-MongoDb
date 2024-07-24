using Base.Domain.ApplicationSettings.Security;
using Base.Domain.Utils.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Utils.Extentions.Security
{
    public static class TokenExtentions
    {
        public static (string token, DateTime expires) GenerateJwtToken(bool rememberMe, params Claim[] claims)
        {
            //Setting
            var jwtSettings = SingleTon<AppSettings>.Instance.Get<JwtSettings>();


            //X509 Certificate Desciptor
            var certificate = new X509Certificate2(
              jwtSettings.FilePath,
              jwtSettings.ExportKey,
              X509KeyStorageFlags.MachineKeySet |
              X509KeyStorageFlags.PersistKeySet |
              X509KeyStorageFlags.Exportable
            );


            //header
            string Issuer = jwtSettings.Issuer;
            string Audience = jwtSettings.Audience;
            var expires = DateTime.UtcNow.Add(jwtSettings.ExpireTime);
            if (rememberMe)
                expires.AddDays(jwtSettings.RememberMeDayCount);

            var key = new X509SecurityKey(certificate);

            //signiture
            var signingCredentials = new SigningCredentials(key, jwtSettings.HashAlgorithm);

            //payload
            var subject = new ClaimsIdentity(claims);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                //Headers
                Issuer = Issuer,
                Audience = Audience,
                Expires = expires,

                //payload
                Subject = subject,

                //signiture
                SigningCredentials = signingCredentials,
            };

            var tokenHandler = new JwtSecurityTokenHandler()
            {
                MapInboundClaims = false,
                OutboundClaimTypeMap = new Dictionary<string, string>()
            };
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            string JwtToken = tokenHandler.WriteToken(token);

            return (JwtToken, expires);

        }

        public static (string token, DateTime expires) GenerateRefreshToken(bool rememberMe)
        {
            return GenerateJwtToken(rememberMe);
        }


        public static string GetJwtTokenFromHttpContext(this HttpContext httpContext)
        {
            return httpContext.Request.Headers["Authorization"].FirstOrDefault().Replace("Bearer ", "");
        }
    }
}
