using System;
namespace StateBuilder.Library.Infrastructure.SystemAttributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Constructor, AllowMultiple = false)]
    public class RootWorkItem :  Attribute
    {
        Type _type = null;
        public RootWorkItem(Type type)
        {
            if (type == typeof(IWorkItem))
            {
                IWorkItem rootWorkItem = WorkItemExtensions.RootWorkItem;
                _type = type;
            }
        }
    }
}