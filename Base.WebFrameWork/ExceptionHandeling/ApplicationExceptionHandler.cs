using Base.Domain.DomainExceptions;
using Base.Domain.Models.OutPutModels;
using Base.Domain.RepositoriesApi.Core;
using Base.Infrasucture.Connections.Api;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Base.WebFrameWork.ExceptionHandeling
{
    public class ApplicationExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
        {
#if DEBUG
            return false;
#endif
            if (exception != null)
            {
                context.RequestServices.GetRequiredService<IBaseCore>().AbortTransactionMain();
                context.RequestServices.GetRequiredService<IBaseDbConnectionContext>().Dispose();
                context.Response.Clear();
                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                context.Response.ContentType = Text.Plain;
                if (exception is DomainException)
                {
                    ApiResult result = new ApiResult()
                    {
                        Messages = new List<string> { exception.Message },
                        StatusCode = StatusCodes.Status400BadRequest,
                        Success = false
                    };

                    await context.Response.WriteAsJsonAsync(result, cancellationToken);
                    await context.Response.CompleteAsync();
                    return true;
                }
                else
                {
                    ElmahCore.ElmahExtensions.RaiseError(context, exception);

                    ApiResult result = new ApiResult()
                    {
                        Messages = new List<string> { "خطایی رخ داد! لطفا به پشتیبانی اطلاع دهید!" },
                        StatusCode = StatusCodes.Status400BadRequest,
                        Success = false
                    };

                    await context.Response.WriteAsJsonAsync(result);
                    await context.Response.CompleteAsync();

                    return true;
                }
            }

            return false;
        }
    }
}
