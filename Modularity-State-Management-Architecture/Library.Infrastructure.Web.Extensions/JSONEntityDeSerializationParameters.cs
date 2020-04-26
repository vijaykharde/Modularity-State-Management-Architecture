using StateBuilder.Library.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Infrastructure.Web.Extensions
{
    public class JSONEntityDeSerializationParameters
    {
        public IWorkItem WorkItem { get; set; }
        public string StateName { get; set; }
        public object DeSerializedEntity { get; set; }
        public dynamic JSONObject { get; set; }
    }
}
