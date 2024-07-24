using Base.Domain.Enums.Base.User;
using Base.Domain.RepositoriesApi.Core;
using Base.Domain.RepositoriesApi.UserInfo;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.RepositoriesApi.Context
{
    public class DomainValidationContext
    {
        private readonly IServiceProvider _serviceProvider;
        private IBaseCore _mainCore;
        private IBaseUserInfoContext _baseUserInfoContext;
        private IUserInfoContext _userInfoContext;
        private ICustomerInfoContext _customerInfoContext;
        private ITenantCore _customerCore;
        public DomainValidationContext(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

        }

        public IBaseCore Core
        {
            get
            {
                if (_mainCore is null)
                    _mainCore = _serviceProvider.GetRequiredService<IBaseCore>();

                return _mainCore;
            }
        }
        public IUserInfoContext CurrentUser
        {
            get
            {
                if (BaseUserInfoContext.UserType == UserType.AdminUser)
                    if (_userInfoContext is null)
                        _userInfoContext = _serviceProvider.GetRequiredService<IUserInfoContext>();

                return _userInfoContext;
            }
        }
        public IBaseUserInfoContext BaseUserInfoContext
        {
            get
            {
                if (_baseUserInfoContext is null)
                    _baseUserInfoContext = _serviceProvider.GetRequiredService<IBaseUserInfoContext>();

                return _baseUserInfoContext;
            }
        }
        public ICustomerInfoContext CustomerInfoContext
        {
            get
            {
                if (BaseUserInfoContext.UserType == UserType.Customer)
                    if (_customerInfoContext is null)
                        _customerInfoContext = _serviceProvider.GetRequiredService<ICustomerInfoContext>();

                return _customerInfoContext;
            }
        }

        public ITenantCore CustomerCore
        {
            get
            {
                if (BaseUserInfoContext.UserType == UserType.Customer)
                    if (_customerCore is null)
                        _customerCore = _serviceProvider.GetRequiredService<ITenantCore>();

                return _customerCore;
            }
        }

    }
}
