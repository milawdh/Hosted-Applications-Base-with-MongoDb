using Base.Domain.RepositoriesApi.Context;
using Base.Domain.RepositoriesApi.Core;
using Base.Domain.RepositoriesApi.UserInfo;
using Base.Infrasucture.Connections.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Infrasucture.Repository.Impl.Context
{
    public class TenantCore : Core, ITenantCore
    {
        public TenantCore(ITenantDbConnectionContext cutomerDbConnectionContext, IBaseUserInfoContext baseUserInfoContext, DomainValidationContext validationContext)
            : base(cutomerDbConnectionContext, baseUserInfoContext, validationContext)
        {
        }
    }
}
