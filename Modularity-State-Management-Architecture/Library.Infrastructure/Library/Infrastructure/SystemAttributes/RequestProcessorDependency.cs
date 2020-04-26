using System;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
namespace StateBuilder.Library.Infrastructure.SystemAttributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Constructor, AllowMultiple = true)]
    public class RequestProcessorDependency : Attribute
    {
        public RequestProcessorDependency(Type classType)
        {

        }
    }
}