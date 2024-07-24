using Base.Domain.Entities.Base.Users;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.RepositoriesApi.UserInfo
{
    public interface ICustomerInfoContext
    {
        public ObjectId CustomerId { get; }
        public CustomerCollection Customer { get; }
    }
}
