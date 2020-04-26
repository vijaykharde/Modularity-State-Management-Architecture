using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using StateBuilder.Library.Infrastructure;
using System.Threading.Tasks;
using Unity;
namespace Library.Infrastructure.Web.Extensions
{
    public class HttpContextMiddleware
    {
        private readonly RequestDelegate _next;
        public HttpContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            var obj = httpContext.RequestServices.GetRequiredService(typeof(IWorkItem));
            if (obj != null)
            {
                IWorkItem wi = obj as IWorkItem;
                if (wi != null)
                {
                    //bool isRegisteredHttpContextAccessor = wi.RootWorkItem.Container.IsRegistered<IHttpContextAccessor>();
                    //if (!isRegisteredHttpContextAccessor)
                    //{
                    //    wi.RootWorkItem.Container.RegisterInstance<IHttpContextAccessor>(new HttpContextAccessor());
                    //}

                    //IHttpContextAccessor httpContextAccessor = wi.RootWorkItem.Container.Resolve(typeof(IHttpContextAccessor), null, null) as IHttpContextAccessor;
                    IHttpContextAccessor httpContextAccessor = wi.Container.Resolve(typeof(IHttpContextAccessor), null, null) as IHttpContextAccessor;
                    httpContextAccessor.HttpContext = httpContext;
                }
            }
            return _next(httpContext);
        }
    }
}
