using System;

namespace StateBuilder.RequestManager
{
    public class RequestContext<TRequest, TResponse>
    {
        public TRequest Request { get; set; }
        public TResponse Response { get; set; }
        public Action<RequestContext<TRequest, TResponse>> OnCompleteCallback { get; set; }
        public Action<RequestContext<TRequest, TResponse>, Exception> OnExceptionCallback { get; set; }
        public RequestMetricData[] Metrics { get; set; }
    }
}
