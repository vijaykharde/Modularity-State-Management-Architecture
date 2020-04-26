using System;

namespace StateBuilder.RequestManager
{
    public class RequestObserver : IRequestObserver
    {
        //private readonly IApiLoggingService _apiLoggingService;
        //private readonly INotificationService _notificationService;

        //public RequestObserver(IApiLoggingService apiLoggingService, INotificationService notificationService)
        //{
        //    _apiLoggingService = apiLoggingService;
        //    _notificationService = notificationService;
        //}

        public void OnRequestCompleted<TRequest, TResponse>(RequestContext<TRequest, TResponse> requestContext)
        {
            //if (requestContext is ApiCallRequestContext<TRequest, TResponse> apiCallRequestContext && apiCallRequestContext.Metrics != null)
            //{
            //    foreach (var metricData in apiCallRequestContext.Metrics)
            //    {
            //        TelemetryRecorder.TrackApiMetric(
            //            apiCallRequestContext.ServiceName,
            //            apiCallRequestContext.ApiCallName,
            //            metricData.MetricName,
            //            metricData.MetricValue,
            //            sampleTime:
            //            metricData.SampleTime
            //        );
            //    }
            //    _apiLoggingService?.Log(apiCallRequestContext);
            //}
        }

        public void OnRequestFailed<TRequest, TResponse>(RequestContext<TRequest, TResponse> requestContext, Exception exception)
        {
            //if (requestContext is ApiCallRequestContext<TRequest, TResponse> apiCallRequestContext)
            //{
            //    TelemetryRecorder.TrackApiError(
            //        apiCallRequestContext.ServiceName,
            //        apiCallRequestContext.ApiCallName,
            //        apiCallRequestContext.HttpStatusCode,
            //        apiCallRequestContext.ApiRequestContent,
            //        apiCallRequestContext.ApiResponseContent,
            //        exception,
            //        invokeTime:
            //        apiCallRequestContext.InvokeTime
            //    );
            //    if (!(exception is TaskCanceledException))
            //    {
            //        _notificationService.ShowNotification(new ErrorNotificationViewModel
            //        {
            //            Message = $"{apiCallRequestContext.ServiceName}+{apiCallRequestContext.ApiCallName} : {exception.Message}"
            //        });
            //    }
            //}
            //else
            //{
            //    TelemetryRecorder.TrackException(exception);
            //    if (!(exception is TaskCanceledException))
            //    {
            //        _notificationService.ShowNotification(new ErrorNotificationViewModel
            //        {
            //            Message = $"{requestContext.Request.GetType().Name} : {exception.Message}"
            //        });
            //    }
            //}
        }

        public void OnRequestStarted<TRequest, TResponse>(RequestContext<TRequest, TResponse> requestContext)
        {
        }

        public void OnRequestSucceeded<TRequest, TResponse>(RequestContext<TRequest, TResponse> requestContext)
        {
        }
    }
}
