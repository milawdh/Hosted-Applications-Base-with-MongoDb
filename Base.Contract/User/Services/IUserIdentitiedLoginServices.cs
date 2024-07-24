using Base.Domain.Entities.Base.Users;
using Base.Domain.Models.OutPutModels;
using Base.Domain.Shared.ViewModels.Token;
using Base.Domain.Shared.ViewModels.UserLogin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Contract.User.Services
{
    public interface IUserIdentitiedLoginServices
    {
        ServiceResult CheckUserLockOut(UserCollection user);
        ServiceResult<UserCollection> ValidateLogin(UseroginRequestDto model);
        ServiceResult LogOut();
        ServiceResult<LoginResulDto> OperateLogin(UserCollection user, BaseUserLoginRequestDto model);
        ServiceResult OperateNewCode(string phoneNumber);
        ServiceResult RequestForgotPassword(UserForgotPasswordRequestDto model);
        ServiceResult<RefreshTokenResultDto> OperateRefreshToken(RefreshTokenRequestDto refreshTokenRequestDto);
        ServiceResult OperateRegisterEmail(RegisterEmailRequestDto model);

        ServiceResult OperateRefreshPassword(ForgotPasswordSubmitCode model);
    }
}
