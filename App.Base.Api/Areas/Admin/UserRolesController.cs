using Base.Contract.Roles;
using Base.Domain.Models.Query;
using Base.Domain.Shared.ViewModels.Roles;
using Base.WebFrameWork.Attributes.Authorization;
using Base.WebFrameWork.ControllerModels;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace App.Base.Api.Areas.Admin
{
    [Area("Admin")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    [UserPermission]
    public class UserRolesController : BaseApiController
    {
        private readonly IUserRoleService _roleService;

        public UserRolesController(IUserRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet("GetAutoComplete")]
        public IActionResult GetAutoComplete(string q)
        {
            return OkResult(_roleService.GetAutoComplete(q));
        }

        [HttpGet("GetList")]
        public IActionResult GetList([FromQuery] SearchRequestModel model)
        {
            return OkResult(_roleService.GetList(model).AsPagedModel());
        }

        [HttpGet("GetId/{Id}")]
        [UserPermission("GetList")]
        public async Task<IActionResult> GetId(int Id)
        {
            var checkAccess = await _roleService.CheckAccessAsync(Id, true);
            if (checkAccess.IsFailed)
                return SmartResult(checkAccess);


            var res = checkAccess.Result.Adapt<UserRoleDto>();
            return OkResult(res);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add(CreateUpdateUserRoleDto model)
        {
            return SmartResult(await _roleService.AddAsync(model));
        }


        [HttpPut("Update/{Id}")]
        public async Task<IActionResult> Update(int Id, [FromBody] CreateUpdateUserRoleDto model)
        {
            var checkAccess = await _roleService.CheckAccessAsync(Id, true);
            if (checkAccess.IsFailed)
                return SmartResult(checkAccess);

            return SmartResult(await _roleService.UpdateAsync(model, checkAccess.Result));
        }

        [HttpDelete("Delete/{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            var checkAccess = await _roleService.CheckAccessAsync(Id, true);
            if (checkAccess.IsFailed)
                return SmartResult(checkAccess);

            return SmartResult(await _roleService.DeleteAsync(checkAccess.Result));
        }
    }
}
