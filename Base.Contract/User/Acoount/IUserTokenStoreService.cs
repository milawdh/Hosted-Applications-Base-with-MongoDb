using Base.Domain.Entities.Base.Users;
using Base.Domain.Enums.Base.User;
using Base.Domain.Models.OutPutModels;
using Base.Domain.Shared.ViewModels.Token;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Contract.User.Acoount
{
    public interface IUserTokenStoreService
    {
        (TokenStoreCollection tokenModel, string refreshToken, string jwtToken) GenenrateUserTokenModel(ObjectId userId, bool RememberMe, UserType userType);
        ServiceResult<(TokenStoreCollection tokenModel, string refreshToken, string jwtToken)> RefreshToken(RefreshTokenRequestDto model);
        ServiceResult<TokenStoreCollection> ValidateToken(string jwtToken, UserType? userType = null);
    }
}
