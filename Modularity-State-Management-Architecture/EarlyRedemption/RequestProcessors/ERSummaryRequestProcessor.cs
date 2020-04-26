using EarlyRedemption.BusinessObjects;
using StateBuilder.Library.Infrastructure;
using StateBuilder.RequestManager;
using System.Threading;
using System.Threading.Tasks;

namespace EarlyRedemption.RequestProcessors
{
    public class ERSummaryRequestProcessor : IRequestProcessor<ERQueryObject, ERResponseObject>
    {
        public void AssertRequest(ERQueryObject request)
        {
            //throw new System.NotImplementedException();
        }

        public Task ProcessAsync(RequestContext<ERQueryObject, ERResponseObject> requestContext, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                requestContext.Response = new ERResponseObject()
                {
                    ProductType = requestContext.Request.ProductType,
                    Currency = requestContext.Request.Currency
                };
            });
        }
    }
}
