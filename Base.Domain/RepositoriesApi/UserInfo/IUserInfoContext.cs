using Base.Domain.Entities.Base.Users;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.RepositoriesApi.UserInfo
{
    public interface IUserInfoContext
    {
        public ObjectId UserId { get; }
        public UserCollection User { get; }
    }
}
