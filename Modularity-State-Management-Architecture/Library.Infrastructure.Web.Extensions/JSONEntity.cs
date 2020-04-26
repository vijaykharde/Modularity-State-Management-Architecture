using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Infrastructure.Web.Extensions
{
    public class JSONEntity
    {
        public string WorkItemId { get; set; }
        public string StateName { get; set; }

        public object StateValue { get; set; }
    }
}
