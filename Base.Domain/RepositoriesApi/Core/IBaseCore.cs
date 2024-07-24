using MongoDB.Bson;
using Base.Domain.Entities.Base.Access;
using Base.Domain.RepositoriesApi.Repo;
using Base.Domain.Entities.Base.Cities;
using Base.Domain.Entities.Base.Sms;
using Base.Domain.Entities.Base.Users;

namespace Base.Domain.RepositoriesApi.Core
{
    public interface IBaseCore : ICore
    {
        IMainRepo<ProvinceCollection, ObjectId> ProvinceMain { get; }

        IMainRepo<CityCollection, ObjectId> CityMain { get; }

        IMainRepo<UserCollection, ObjectId> Users { get; }

        IMainRepo<TokenStoreCollection, ObjectId> UserTokenStoreMain { get; }

        IMainRepo<SmsArchiveCollention, ObjectId> PhoneVerificationMain { get; }

        IMainRepo<UserRoleCollection, int> UserRole { get; }

        IMainRepo<UserPermissionCollection, int> UserPermissions { get; }

    }
}