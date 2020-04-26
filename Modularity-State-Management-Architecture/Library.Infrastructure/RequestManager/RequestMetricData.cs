using System;

namespace StateBuilder.RequestManager
{
    public class RequestMetricData
    {
        public DateTime SampleTime { get; set; }
        public string MetricName { get; set; }
        public double MetricValue { get; set; }
    }
}
