using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.VisualBasic;
using System.Net;
using System.Text.RegularExpressions;

namespace BookmanagementAPI.Filters
{
  
        public class ExceptionMiddleware : ExceptionFilterAttribute
        {
            #region Declaration

            private readonly RequestDelegate _next;
            #endregion

            #region Constructor

            public ExceptionMiddleware(RequestDelegate next)
            {
                _next = next;
            }
            #endregion

            #region invoke exception email
            public async Task InvokeAsync(HttpContext httpContext)
            {
                try
                {
                    await _next(httpContext);
                }
                catch (Exception ex)
                {
                        await HandleException(httpContext, ex);
                }
            }
            #endregion

            #region Handle exception
            private async Task<HttpContext> HandleException(HttpContext context, Exception exception)
            {
                var responseStatus = await Task<HttpContext>.Factory.StartNew(() => context);
                return responseStatus;
            }
            #endregion
        }

        #region Extension method used to add the middle ware to the HTTP request pipeline

        // Extension method used to add the middle-ware to the HTTP request pipeline.
        public static class ExceptionMiddlewareExtensions
        {
            public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
            {
                return builder.UseMiddleware<ExceptionMiddleware>();
            }
        }
        #endregion
    }

