using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;

namespace HireMeApp.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetAntiforgeryTokenForJs(this HttpContext httpContext)
        {
            var antiforgeryService = (IAntiforgery)httpContext.RequestServices.GetService(typeof(IAntiforgery));
            var tokenSet = antiforgeryService.GetAndStoreTokens(httpContext);

            return tokenSet.RequestToken;
        }
    }
}
