using Microsoft.AspNetCore.Http;
using System;
using System.Diagnostics;
using System.Net;

namespace SweetsAndSnacks.Middlewares
{
    public class ExceptionHandlingMiddleware :IMiddleware
    {        
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
        {            
            _logger = logger;
        }

        
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {                
                _logger.LogError(ex, ex.Message);
                /*_logger.LogError(String.Concat(ex.Message," ReqId:", 
                    Activity.Current?.Id ?? httpContext.TraceIdentifier, "\r\n",
                    ex.StackTrace?.Substring(0, ex.StackTrace.IndexOf(":line",0) + 12)));*/
                await context.Response.WriteAsync("Some Error!");
            }
        }
    }
}
