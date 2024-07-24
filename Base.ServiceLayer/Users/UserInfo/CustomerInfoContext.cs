using Base.Domain.DomainExceptions;
using Base.Domain.Entities.Base.Users;
using Base.Domain.Enums.Base.User;
using Base.Domain.RepositoriesApi.UserInfo;

using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.ServiceLayer.Users.UserInfo
{
    public class CustomerInfoContext : ICustomerInfoContext
    {
        //TODO : Implement All of it
        private readonly IMongoDatabase _mainDb;
        private readonly IBaseUserInfoContext _baseUserInfoContext;
        public CustomerInfoContext(IMongoDatabase mainDb, IBaseUserInfoContext baseUserInfoContext)
        {
            _mainDb = mainDb;
            _baseUserInfoContext = baseUserInfoContext;
        }

        private CustomerCollection _customer;

        public ObjectId CustomerId
        {
            get
            {
                if (_baseUserInfoContext.UserType != UserType.Customer)
                    throw new AuthorizationException("Invalid Token!");

                return _baseUserInfoContext.UserId;
            }
        }

        public CustomerCollection Customer
        {
            get
            {
                if (_customer == null)
                {

                    var customer = _mainDb.GetCollection<CustomerCollection>(nameof(CustomerCollection)).Find(a => a.Id == CustomerId).FirstOrDefault();
                    if (customer is null)
                        throw new AuthorizationException("Invalid Token!");

                    _customer = customer;
                }
                return _customer;
            }
        }
    }
}
