using Base.Contract.User.Acoount;
using Base.Domain.ApplicationSettings.Security;
using Base.Domain.ApplicationSettings.User;
using Base.Domain.Entities.Base.Users;
using Base.Domain.Enums.Base.User;
using Base.Domain.Models.OutPutModels;
using Base.Domain.Shared.ViewModels.Token;
using Base.Domain.Utils.Configuration;
using Base.Domain.Utils.Extentions.Security;
using Base.Infrasucture.Connections.Api;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Base.ServiceLayer.Users
{
    public class UserTokenStoreService : IUserTokenStoreService
    {
        private readonly IBaseDbConnectionContext mainDbConnectionContext;

        public UserTokenStoreService(IBaseDbConnectionContext mainDbConnectionContext)
        {
            this.mainDbConnectionContext = mainDbConnectionContext;
        }
        public ServiceResult<TokenStoreCollection> ValidateToken(string jwtToken, UserType? userType = null)
        {
            var jwtSettings = SingleTon<AppSettings>.Instance.Get<JwtSettings>();

            var cert = new X509Certificate2(jwtSettings.FilePath, jwtSettings.ExportKey);

            var validateParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new X509SecurityKey(cert)
            };

            var jwtHandler = new JwtSecurityTokenHandler()
            {
                MapInboundClaims = false,
                OutboundClaimTypeMap = new Dictionary<string, string>(),
                InboundClaimTypeMap = new Dictionary<string, string>(),
            };
            if (!jwtHandler.CanReadToken(jwtToken))
                return new ServiceResult<TokenStoreCollection>("Invalid Token!");

            jwtHandler.ValidateToken(jwtToken, validateParameters, out var securityToken);

            var userClaim = jwtHandler.ReadJwtToken(jwtToken).Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            if (userClaim == null)
                return new ServiceResult<TokenStoreCollection>("Invalid Token!");

            if (!ObjectId.TryParse(userClaim.Value, out var userId))
                return new ServiceResult<TokenStoreCollection>("Invalid Token!");

            jwtToken = jwtToken.HashData();

            var userToken = mainDbConnectionContext.DataBase.GetCollection<TokenStoreCollection>(nameof(TokenStoreCollection)).Find(x => x.TokenHash == jwtToken).FirstOrDefault();
            if (userToken == null)
                return new ServiceResult<TokenStoreCollection>("Invalid Token!");

            if (userToken.TokenExpireDate <= DateTime.UtcNow)
                return new ServiceResult<TokenStoreCollection>("Invalid Token!");

            if (userType != null)
                if (userToken.UserType != userType.Value)
                    return new ServiceResult<TokenStoreCollection>("Invalid Token!");

            switch (userToken.UserType)
            {
                case UserType.AdminUser:
                    var user = mainDbConnectionContext.DataBase
                         .GetCollection<UserCollection>(nameof(UserCollection)).Find(x => x.Id == userId).FirstOrDefault();
                    if (user == null)
                        return new ServiceResult<TokenStoreCollection>("Invalid Token!");

                    break;
                case UserType.Customer:
                    var customer = mainDbConnectionContext.DataBase
                         .GetCollection<CustomerCollection>(nameof(CustomerCollection)).Find(x => x.Id == userId).FirstOrDefault();
                    if (customer == null)
                        return new ServiceResult<TokenStoreCollection>("Invalid Token!");
                    break;
                default:
                    break;
            }
            return new ServiceResult<TokenStoreCollection>(userToken);
        }

        public ServiceResult<(TokenStoreCollection tokenModel, string refreshToken, string jwtToken)> RefreshToken(RefreshTokenRequestDto model)
        {
            var tokenStoreCollection = mainDbConnectionContext.DataBase.GetCollection<TokenStoreCollection>(nameof(TokenStoreCollection));

            model.Token = model.Token.HashData();
            model.RefreshToken = model.RefreshToken.HashData();

            var userToken = tokenStoreCollection
                  .FindOneAndDelete(x => x.RefreshTokenHash == model.RefreshToken && model.Token == x.TokenHash);

            var newToken = GenenrateUserTokenModel(userToken.UserId, userToken.HasRememberMe, userToken.UserType);

            return new ServiceResult<(TokenStoreCollection tokenModel, string refreshToken, string jwtToken)>(newToken);
        }

        public (TokenStoreCollection tokenModel, string refreshToken, string jwtToken) GenenrateUserTokenModel(ObjectId userId, bool RememberMe, UserType userType)
        {
            var loginSettings = SingleTon<AppSettings>.Instance.Get<UserLoginSettings>();
            var tokenStoreCollection = mainDbConnectionContext.DataBase.GetCollection<TokenStoreCollection>(nameof(TokenStoreCollection));

            if (tokenStoreCollection.Find(x => x.UserId == userId).Any())
                tokenStoreCollection.DeleteOne(x => x.UserId == userId);

            var jwtToken = TokenExtentions.GenerateJwtToken(RememberMe, GetUserClaims(userId));
            var refreshToken = TokenExtentions.GenerateRefreshToken(RememberMe);
            TokenStoreCollection userToken = new()
            {
                RefreshTokenHash = refreshToken.token.HashData(),
                TokenHash = jwtToken.token.HashData(),
                RefreshTokenExpireDate = refreshToken.expires,
                TokenExpireDate = jwtToken.expires,
                HasRememberMe = RememberMe,
                UserId = userId,
                UserType = userType,
                CretatedOn = DateTime.UtcNow,
            };

            tokenStoreCollection.InsertOne(userToken);

            return (userToken, refreshToken.token, jwtToken.token);
        }
        private Claim[] GetUserClaims(ObjectId userId)
        {
            return new Claim[]
              {
                new Claim(ClaimTypes.NameIdentifier,userId.ToString())
              };
        }
    }
}
