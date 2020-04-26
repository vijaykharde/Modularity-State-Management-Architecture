using System;

namespace StateBuilder.RequestManager
{
    public class ApiCallRequestContext<TRequest, TResponse> : RequestContext<TRequest, TResponse>
    {
        public string ServiceName { get; set; }
        public string ApiCallName { get; set; }
        public string ApiRequestContent { get; set; }
        public string ApiResponseContent { get; set; }
        public DateTime InvokeTime { get; set; }
        public short? HttpStatusCode { get; set; }
    }
}
