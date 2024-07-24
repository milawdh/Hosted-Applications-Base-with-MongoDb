using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Domain.Models.OutPutModels;

namespace Base.WebFrameWork.ControllerModels
{
    public abstract class BaseApiController : Controller
    {

        /// <summary>
        /// Returns Api Response with Result Object from ServiceLayer
        /// </summary>
        /// <typeparam name="T">Type of Object in ServiceResult</typeparam>
        /// <param name="service">Service Resut With Object From ServiceLayer</param>
        /// <returns></returns>
        protected IActionResult SmartResult<T>(ServiceResult<T> service)
        {
            if (service.IsFailed)
                return BadRequest(new ApiResult<T>(service));

            return Ok(new ApiResult<T>(service));
        }
        /// <summary>
        /// Returns Api Response WithOut Result Object from ServiceLayer
        /// </summary>
        /// <param name="serviceResult">Service Result Without Object From ServiceLayer</param>
        /// <returns></returns>
        protected IActionResult SmartResult(ServiceResult serviceResult)
        {
            if (serviceResult.IsFailed)
                return BadRequest(new ApiResult(serviceResult));

            return Ok(new ApiResult(serviceResult));
        }

        /// <summary>
        /// Returns Response With Errors in ModelState
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        protected IActionResult BadResult(ModelStateDictionary modelState)
        {
            var errors = modelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return BadRequest(new ApiResult(errors));
        }

        /// Returns Response With One Error Message
        protected IActionResult BadResult(string message)
        {
            return BadRequest(new ApiResult(message));
        }

        /// Returns Response With Errors in ModelState
        protected IActionResult BadResult(List<string> errors)
        {
            return BadRequest(new ApiResult(errors));
        }

        /// <summary>
        /// Returns Ok Result 
        /// </summary>
        /// <typeparam name="T">The type of the result object</typeparam>
        /// <param name="value">The result object</param>
        /// <returns></returns>
        protected IActionResult OkResult<T>(T value)
        {
            return Ok(new ApiResult<T>(value));
        }

        /// <summary>
        /// Returns Ok result with custom messages
        /// </summary>
        /// <param name="messages">Message To Send</param>
        /// <returns></returns>
        protected IActionResult OkResult(params string[] messages)
        {
            return Ok(new ApiResult()
            {
                StatusCode = 200,
                Success = true,
                Messages = messages.ToList(),
            });
        }
    }
}
