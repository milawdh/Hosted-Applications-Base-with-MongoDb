using Base.Domain.DomainExceptions;
using Base.Domain.Entities.Base.Users;
using Base.Domain.Enums.Base.User;
using Base.Domain.RepositoriesApi.UserInfo;

using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDbGenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.ServiceLayer.Users.UserInfo
{
    public class UserInfoContext : IUserInfoContext
    {
        //TODO : Implement All of it
        private readonly IBaseUserInfoContext baseUserInfoContext;
        private readonly IMongoDatabase _mainDb;

        public UserInfoContext(IMongoDatabase mainDb, IBaseUserInfoContext baseUserInfoContext)
        {
            this.baseUserInfoContext = baseUserInfoContext;
            _mainDb = mainDb;
        }

        private UserCollection _user;


        public ObjectId UserId
        {
            get
            {
                if (baseUserInfoContext.UserType != UserType.AdminUser)
                    throw new AuthorizationException("Invalid Token!");

                return baseUserInfoContext.UserId;
            }
        }

        public UserCollection User
        {
            get
            {
                if (_user == null)
                {

                    var user = _mainDb.GetCollection<UserCollection>(nameof(UserCollection)).Find(a => a.Id == UserId).FirstOrDefault();
                    if (user is null)
                        throw new AuthorizationException("Invalid Token!");

                    _user = user;
                }
                return _user;
            }
        }

    }
}
