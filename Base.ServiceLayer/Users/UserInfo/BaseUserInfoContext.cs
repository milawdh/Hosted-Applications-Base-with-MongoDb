using Base.Contract.User.Acoount;
using Base.Domain.DomainExceptions;
using Base.Domain.Entities.Base.Users;
using Base.Domain.Enums.Base.User;
using Base.Domain.RepositoriesApi.UserInfo;
using Base.Domain.Utils.Extentions.Security;

using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.ServiceLayer.Users.UserInfo
{
    public class BaseUserInfoContext : IBaseUserInfoContext
    {
        private readonly IMongoDatabase _mainDb;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserTokenStoreService _userTokenStoreService;
        public BaseUserInfoContext(IMongoDatabase mainDb, IHttpContextAccessor httpContextAccessor, IUserTokenStoreService userTokenStoreService)
        {
            _mainDb = mainDb;
            _httpContextAccessor = httpContextAccessor;
            _userTokenStoreService = userTokenStoreService;
        }

        private TokenStoreCollection _userTokenStoreCollection;
        public TokenStoreCollection UserToken
        {
            get
            {
                if (_httpContextAccessor.HttpContext == null)
                {
                    return new TokenStoreCollection()
                    {
                        UserType = UserType.Application,
                        CretatedOn = DateTime.UtcNow,
                        UserId = ObjectId.Empty,
                    };
                }
                if (_userTokenStoreCollection == null)
                {

                    var jwtToken = _httpContextAccessor.HttpContext.GetJwtTokenFromHttpContext();
                    var validateResult = _userTokenStoreService.ValidateToken(jwtToken);
                    if (validateResult.IsFailed)
                        throw new AuthorizationException("InvalidToken!");

                    _userTokenStoreCollection = validateResult.Result;
                }
                return _userTokenStoreCollection;
            }
        }
        public ObjectId UserId
        {
            get
            {
                return UserToken.UserId;
            }
        }

        public UserType UserType
        {
            get
            {
                return UserToken.UserType;
            }
        }
    }
}
