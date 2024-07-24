using Base.Contract.Permission;
using Base.Contract.User.Services;
using Base.Domain.Models.OutPutModels;
using Base.Domain.RepositoriesApi.Core;
using Base.Domain.Shared.ViewModels.Token;
using Base.Domain.Shared.ViewModels.UserLogin;
using Base.WebFrameWork.ControllerModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Swashbuckle.AspNetCore.Annotations;

namespace App.Base.Api.Areas.Admin
{
    [Area("Admin")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    public class AccountController : BaseApiController
    {
        private readonly IUserIdentitiedLoginServices _identitiedUserServices;
        private readonly IBasePermissionService _permissionService;
        private readonly IBaseCore _mainCore;

        public AccountController(IUserIdentitiedLoginServices identitiedUserServices, IBasePermissionService permissionService, IBaseCore mainCore)
        {
            _identitiedUserServices = identitiedUserServices;
            _permissionService = permissionService;
            _mainCore = mainCore;
        }

        [HttpPost("RequestForgotPassword")]
        [SwaggerOperation("درخواست تجدید رمز عبور")]
        [ProducesResponseType<ApiResult>(200)]
        public IActionResult RequestForgotPassword(UserForgotPasswordRequestDto model)
        {
            if (!ModelState.IsValid)
                return BadResult(ModelState);

            var result = _identitiedUserServices.RequestForgotPassword(model);
            return SmartResult(result);
        }

        [HttpPost("SendVerificationCode")]
        [SwaggerOperation("ارسال مجدد کد تاییدیه ایمیل")]
        [ProducesResponseType<ApiResult>(200)]
        public IActionResult SendVerificationCode(string email)
        {
            if (!ModelState.IsValid)
                return BadResult(ModelState);

            var codeResult = _identitiedUserServices.OperateNewCode(email);
            return SmartResult(codeResult);
        }

        [HttpPost("Login")]
        [SwaggerOperation("ورود کاربر")]
        [ProducesResponseType<ApiResult<LoginResulDto>>(200)]
        public IActionResult Login(UseroginRequestDto model)
        {
            if (!ModelState.IsValid)
                return BadResult(ModelState);

            var userResult = _identitiedUserServices.ValidateLogin(model);
            if (userResult.IsFailed)
                return SmartResult(userResult);

            var loginResult = _identitiedUserServices.OperateLogin(userResult.Result, model);
            return SmartResult(loginResult);
        }

        [Authorize]
        [HttpPut("Logout")]
        [ProducesResponseType<ApiResult>(200)]
        public IActionResult Logout()
        {
            _permissionService.CheckPermission();
            var result = _identitiedUserServices.LogOut();

            return SmartResult(result);
        }

        [Authorize]
        [HttpGet("RefreshToken")]
        [ProducesResponseType<ApiResult<RefreshTokenRequestDto>>(200)]
        public IActionResult RefreshToken([FromQuery] RefreshTokenRequestDto model)
        {
            var result = _identitiedUserServices.OperateRefreshToken(model);
            return SmartResult(result);
        }

    }
}
