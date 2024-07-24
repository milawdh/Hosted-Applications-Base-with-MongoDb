using Base.Domain.Audities;
using Base.Domain.Enums.Base.User;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Entities.Base.Users
{

    public class TokenStoreCollection : BaseEntity
    {
        public string TokenHash { get; set; }

        public string RefreshTokenHash { get; set; }

        public UserType UserType { get; set; }

        public DateTime TokenExpireDate { get; set; }

        public DateTime RefreshTokenExpireDate { get; set; }

        public bool HasRememberMe { get; set; }

        public ObjectId UserId { get; set; }
        public DateTime CretatedOn { get; set; } = DateTime.UtcNow;
    }
}
