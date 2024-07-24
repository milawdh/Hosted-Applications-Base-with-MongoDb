using Base.Contract.Sms;
using Base.Contract.User.Acoount;
using Base.Contract.User.Services;
using Base.Domain.ApplicationSettings.User;
using Base.Domain.Entities.Base.Users;
using Base.Domain.Enums.Base.User;
using Base.Domain.Models.OutPutModels;
using Base.Domain.RepositoriesApi.Core;
using Base.Domain.RepositoriesApi.UserInfo;
using Base.Domain.Shared.ViewModels.Token;
using Base.Domain.Shared.ViewModels.UserLogin;
using Base.Domain.Utils.Configuration;
using Base.Domain.Utils.Extentions.Security;
using Mapster;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq.Expressions;

namespace Base.ServiceLayer.Users
{
    public class UserIdentitiedLoginServices : IUserIdentitiedLoginServices
    {
        private readonly ISmsService _verificationSmsManager;
        private readonly IBaseUserInfoContext _baseUserInfoContext;
        private readonly IUserTokenStoreService _userTokenStoreService;
        private readonly IBaseCore _core;

        public UserIdentitiedLoginServices(IBaseCore core, ISmsService verificationSmsService, IBaseUserInfoContext userInfoContext, IUserTokenStoreService userTokenStoreService)
        {
            _core = core;
            _verificationSmsManager = verificationSmsService;
            _baseUserInfoContext = userInfoContext;
            _userTokenStoreService = userTokenStoreService;
        }

        #region Query

        /// <summary>
        /// The Query of Entities That We Have Access By the Expression!
        /// </summary>
        private IMongoQueryable<UserCollection> QueryManagement
        {
            get
            {
                return _core.Users.AsQuerable().Where(Expression);
            }
        }

        /// <summary>
        /// The Expression of the accessibility to entities
        /// </summary>
        private Expression<Func<UserCollection, bool>> Expression => x => !x.IsDeleted;

        #endregion

        #region Register Operations
        public ServiceResult OperateRegisterEmail(RegisterEmailRequestDto model)
        {
            var settings = SingleTon<AppSettings>.Instance.Get<UserLoginSettings>();

            //Check User Exist
            var user = _core.Users.Get(x => x.UserName == model.UserName).FirstOrDefault();
            if (user == null)
                return new ServiceResult<LoginResulDto>("کاربری با این ایمیل وجود ندارد!");

            if (user.EmailConfirmed)
                return new ServiceResult<LoginResulDto>("کاربر دیگری با این ایمیل ثبت نام خود را تکمیل کرده است!");

            var checkCodeResult = CheckVerificationCodeValidity(user, model.Code);
            if (checkCodeResult.IsFailed)
                return new ServiceResult<LoginResulDto>(checkCodeResult);

            user.EmailConfirmed = true;

            var replaceResult = _core.Users.ReplaceOneImplicit(user.Id, user);
            if (replaceResult.IsFailed)
                return new ServiceResult<LoginResulDto>(replaceResult);

            var userToken = _userTokenStoreService.GenenrateUserTokenModel(user.Id, false, UserType.AdminUser);

            return new ServiceResult();
        }

        public ServiceResult RequestForgotPassword(UserForgotPasswordRequestDto model)
        {
            var settings = SingleTon<AppSettings>.Instance.Get<UserLoginSettings>();

            //Check User Exist
            var user = _core.Users.Get(x => x.UserName == model.UserName).FirstOrDefault();
            if (user == null)
                return new ServiceResult("کاربری با این نام کاربری وجود ندارد!");

            var sendCodeResult = OperateNewCode(user);
            if (sendCodeResult.IsFailed)
                return new ServiceResult(sendCodeResult);

            var replaceResult = _core.Users.ReplaceOneImplicit(user.Id, user);
            if (replaceResult.IsFailed)
                return new ServiceResult(replaceResult);

            return new ServiceResult(isSuccess: true, message: "رمز عبور موقت برایتان ارسال شد");
        }

        public ServiceResult OperateRefreshPassword(ForgotPasswordSubmitCode model)
        {
            //Check User Exist
            var user = _core.Users.Get(x => x.UserName == model.UserName).FirstOrDefault();
            if (user == null)
                return new ServiceResult("اطلاعات وارد شده غلط میباشد!");

            if (!user.EmailConfirmed)
                return new ServiceResult("ایمیل کاربر مورد نظر تایید نشده است! لطفا مجددا به ثبت نام خود اقدام نمایید!");

            var verifyCodeResult = CheckVerificationCodeValidity(user, model.Code);
            if (verifyCodeResult.IsFailed)
                return new ServiceResult(verifyCodeResult);

            user.PasswordHash = model.Password.HashData();

            user.LockoutEnabled = false;
            user.AccessFailedCount = 0;
            user.LockoutEnd = null;

            user.VerificationCodeExpireDate = null;
            user.VerificationCode = null;

            var replaceResult = _core.Users.ReplaceOneImplicit(x => x.Id == user.Id, user);
            if (!replaceResult.IsFailed)
                return new ServiceResult(replaceResult);


            return new ServiceResult();
        }

        #endregion

        #region Login Operations

        /// <summary>
        /// Operates All Login Operations!
        /// </summary>
        /// <param name="user">User That Attempts To Login</param>
        /// <param name="model">Login Request</param>
        /// <returns>Login Result Such as Tokens and User Info</returns>
        public ServiceResult<LoginResulDto> OperateLogin(UserCollection user, BaseUserLoginRequestDto model)
        {
            var userToken = _userTokenStoreService.GenenrateUserTokenModel(user.Id, model.RememberMe, UserType.AdminUser);

            user.AccessFailedCount = 0;
            user.LockoutEnd = null;
            user.LockoutEnabled = false;
            _core.Users.ReplaceOneImplicitAsync(x => x.Id == user.Id, user);

            var result = new LoginResulDto()
            {
                RefreshToken = userToken.refreshToken,
                Token = userToken.jwtToken,
                UserDetails = user.Adapt<UserIdentitiedDetailsDto>()
            };

            return new ServiceResult<LoginResulDto>(result);
        }

        /// <summary>
        /// Gets User That Attempting To Login By the PhoneNumber!
        /// </summary>
        /// <param name="model">Login Request</param>
        /// <returns></returns>
        public ServiceResult<UserCollection> ValidateLogin(UseroginRequestDto model)
        {
            var loginSettings = SingleTon<AppSettings>.Instance.Get<UserLoginSettings>();
            UserCollection user = QueryManagement.Where(x => x.UserName == model.UserName).FirstOrDefault();

            //User Not Found
            if (user == null)
                return new ServiceResult<UserCollection>("رمز عبور یا نام کاربری وارد شده اشتباه است!");

            var lockResult = CheckUserLockOut(user);
            if (lockResult.IsFailed)
                return new ServiceResult<UserCollection>(lockResult);

            //User Password Does Not Match!
            if (user.PasswordHash != model.Password.HashData())
            {
                user.AccessFailedCount++;
                if (user.AccessFailedCount > loginSettings.AllowedAccessFailedCount)
                {
                    user.LockoutEnabled = true;
                    user.LockoutEnd = DateTime.UtcNow.AddMinutes(loginSettings.AccessFailedLockoutMinutes);
                }

                _core.Users.ReplaceOneImplicitAsync(x => x.Id == user.Id, user);
                return new ServiceResult<UserCollection>("رمز عبور یا نام کاربری وارد شده اشتباه است!");
            }

            return new ServiceResult<UserCollection>(user);
        }

        /// <summary>
        /// Checks User Account Is Locked For(It can be any Reason Such as OverAttemptin or another reasons) Login Or Not!
        /// </summary>
        /// <param name="user">User To Check LockOut</param>
        /// <returns></returns>
        public ServiceResult CheckUserLockOut(UserCollection user)
        {
            var loginSettings = SingleTon<AppSettings>.Instance.Get<UserLoginSettings>();

            if (user.AccessFailedCount > loginSettings.AllowedAccessFailedCount)
                user.LockoutEnabled = true;

            //User Account is Locked!
            if (user.LockoutEnabled)
            {
                user.AccessFailedCount++;
                if (user.LockoutEnd.Value.DateTime > DateTime.UtcNow)
                    user.LockoutEnd = DateTime.UtcNow.AddMinutes(loginSettings.AccessFailedLockoutMinutes);

                _core.Users.ReplaceOneImplicitAsync(x => x.Id == user.Id, user);
                return new ServiceResult<UserCollection>("حساب کاربری شما به دلیل تعداد درخواست های ناموفق زیاد مسدود شده است! لطفا دقایقی دیگر امتحان نمایید!");
            }

            return new ServiceResult();
        }


        /// <summary>
        /// Operates All LogOut Operations In Current User!
        /// </summary>
        /// <returns></returns>
        public ServiceResult LogOut()
        {
            var currentUserToken = _baseUserInfoContext.UserToken;

            var deleteResult = _core.UserTokenStoreMain.DeleteForceImplicit(currentUserToken);
            if (deleteResult.IsFailed)
                return deleteResult;

            return new ServiceResult();
        }

        /// <summary>
        /// Operates All Refreshig Token of Current User!
        /// </summary>
        /// <param name="refreshTokenRequestDto">Refresh Token Request Dto!</param>
        /// <returns>A New RefreshToken and Jwt Token</returns>
        public ServiceResult<RefreshTokenResultDto> OperateRefreshToken(RefreshTokenRequestDto refreshTokenRequestDto)
        {
            if (_baseUserInfoContext.UserToken.RefreshTokenHash != refreshTokenRequestDto.RefreshToken.HashData())
                return new ServiceResult<RefreshTokenResultDto>("Invail Token");

            var refreshTokenResult = _userTokenStoreService.RefreshToken(refreshTokenRequestDto);
            if (refreshTokenResult.IsFailed)
                return new ServiceResult<RefreshTokenResultDto>(refreshTokenResult);

            return new ServiceResult<RefreshTokenResultDto>(new RefreshTokenResultDto
            {
                RefreshToken = refreshTokenResult.Result.refreshToken,
                Token = refreshTokenResult.Result.jwtToken
            });
        }

        #endregion

        #region Email Verification

        /// <summary>
        /// Checks Validation And Send Verification Code
        /// </summary>
        /// <param name="phoneNumber">Phone number to send verificationCode</param>
        /// <returns></returns>
        public ServiceResult OperateNewCode(string email)
        {
            var user = _core.Users.Get(x => x.Email == email).FirstOrDefault();
            return OperateNewCode(user);
        }

        /// <summary>
        /// Checks Validation And Send Verification Code
        /// </summary>
        /// <param name="user">User to send verificationCode</param>
        /// <returns></returns>
        private ServiceResult OperateNewCode(UserCollection user)
        {
            var validateResult = CheckUserValidToSendCode(user);
            if (validateResult.IsFailed)
                return validateResult;

            var sendResult = SendVerificationCode(user);
            if (sendResult.IsFailed)
                return sendResult;

            var saveResult = _core.Users.ReplaceOneImplicit(x => x.Id == user.Id, user);
            if (saveResult.IsFailed)
                return saveResult;

            return new ServiceResult();
        }

        /// <summary>
        /// Checks if user valid to send verificationCode to it!
        /// </summary>
        /// <param name="user">User To Check Validity</param>
        /// <returns></returns>
        private ServiceResult CheckUserValidToSendCode(UserCollection user)
        {
            if (user == null || user is default(UserCollection) || user.Id == ObjectId.Empty)
                return new ServiceResult<(string code, DateTime expireTim)>("کاربر مورد نظر یافت نشد");

            var loginSettings = SingleTon<AppSettings>.Instance.Get<UserLoginSettings>();

            if (user == null)
                return new ServiceResult<(string code, DateTime expireTim)>("کاربری با این ایمیل یافت نشد!");

            if (user.VerificationCodeExpireDate > DateTime.UtcNow)
                return new ServiceResult("کد شما ارسال شده است!");

            if (user.VerificationDailyRetry > loginSettings.CodeDailyRetry)
                return new ServiceResult<(string code, DateTime expireTim)>("تعداد درخواست مجاز را رد کرده اید! لطفا بعدا امتحان نمایید یا درصورت نیاز با پشتیبانی تماس بگیرید!");

            return new ServiceResult();
        }

        /// <summary>
        /// Send Verification Sms Code To User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private ServiceResult SendVerificationCode(UserCollection user, int? customExpireMinutes = null)
        {
            var loginSettings = SingleTon<AppSettings>.Instance.Get<UserLoginSettings>();

            user.VerificationCode = GenerateCode();
            user.VerificationCodeExpireDate = DateTime.UtcNow.AddMinutes(customExpireMinutes ?? loginSettings.CodeExpireMinute);
            user.VerificationDailyRetry++;
            //TODO : Implement Sending!

            return new ServiceResult();
        }

        /// <summary>
        /// Generates Code String
        /// </summary>
        /// <returns></returns>
        private string GenerateCode()
        {
            return new Random().Next(10000, 99999).ToString();
        }

        /// <summary>
        /// Checks If the given verification code is valid for the user or not! and Appends All the needed Changes on the User!
        /// Need To Replace User Then!
        /// </summary>
        /// <param name="user">User to check validity</param>
        /// <param name="code">The Code need to validate</param>
        /// <returns></returns>
        private ServiceResult CheckVerificationCodeValidity(UserCollection user, string code)
        {
            //Check Code Expiration
            if (user.VerificationCodeExpireDate <= DateTime.UtcNow)
                return new ServiceResult<LoginResulDto>("زمان ارسال کد به اتمام رسیده است! لطفا کد جدید درخواست نمایید!");

            //Check Code Is Valid
            if (user.VerificationCode == code)
            {
                var lockResult = CheckUserLockOut(user);
                if (lockResult.IsFailed)
                    return new ServiceResult<LoginResulDto>(lockResult);

                user.VerificationCode = "";
                user.VerificationCodeExpireDate = null;
            }

            //If Code Is Not Valid Mark As AccessFailed
            else
            {
                user.AccessFailedCount++;
                _core.Users.ReplaceOneImplicit(user.Id, user);
                return new ServiceResult<LoginResulDto>("کد ارسالی نامعتبر میباشد!");
            }

            return new ServiceResult();
        }

        #endregion
    }
}
