using Base.Domain.Entities.Base.Users;
using Base.Domain.Enums.Base.User;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.RepositoriesApi.UserInfo
{
    public interface IBaseUserInfoContext
    {
        public ObjectId UserId { get; }
        public TokenStoreCollection UserToken { get; }
        public UserType UserType { get; }
    }
}
