using Microsoft.AspNetCore.Builder;

namespace Library.Infrastructure.Web.Extensions
{
    public static class HttpContextMiddlewareExtensions
    {
        public static IApplicationBuilder UseInterceptorMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HttpContextMiddleware>();
        }
    }
}
