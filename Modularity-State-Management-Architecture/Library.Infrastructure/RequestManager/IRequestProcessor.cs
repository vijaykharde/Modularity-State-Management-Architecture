using System.Threading;
using System.Threading.Tasks;

namespace StateBuilder.RequestManager
{
    public interface IRequestProcessor<TRequest, TResponse>
    {
        void AssertRequest(TRequest request);
        Task ProcessAsync(RequestContext<TRequest, TResponse> requestContext, CancellationToken cancellationToken);
    }

    public interface IRequestProcessor<TRequest, TResponse, in TRequestContext> : IRequestProcessor<TRequest, TResponse>
        where TRequestContext : RequestContext<TRequest, TResponse>
    {
        Task ProcessAsync(TRequestContext requestContext, CancellationToken cancellationToken);
    }
}
