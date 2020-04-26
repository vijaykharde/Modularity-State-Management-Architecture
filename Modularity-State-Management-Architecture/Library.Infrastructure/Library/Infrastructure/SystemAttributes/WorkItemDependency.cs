using System;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace StateBuilder.Library.Infrastructure.SystemAttributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Constructor, AllowMultiple = false)]
    public class WorkItemDependency : Attribute
    {
        public WorkItemDependency(Type classType, Type workItemType)
        {
            if (workItemType != null && classType != null)
            {
                IWorkItem classWorkItem = WorkItemExtensions.RootWorkItem.WorkItems.Get(classType);
                if (classWorkItem == null)
                {
                    classWorkItem = CreateWorkItem(classType, WorkItemExtensions.RootWorkItem);
                    if (classWorkItem.Status == WorkItemStatus.NotRunning)
                    {
                        classWorkItem.Run();
                    }
                }

                IWorkItem childWorkItem = classWorkItem.WorkItems.Get(workItemType);
                if (childWorkItem == null)
                {
                    childWorkItem = CreateWorkItem(workItemType, classWorkItem);
                }
                if (childWorkItem.Status == WorkItemStatus.NotRunning)
                {
                    childWorkItem.Run();
                }
            }
        }

        public WorkItemDependency(Type classType)
        {
            IWorkItem classWorkItem = WorkItemExtensions.RootWorkItem.WorkItems.Get(classType);
            if (classWorkItem == null)
            {
                classWorkItem = CreateWorkItem(classType, WorkItemExtensions.RootWorkItem);
            }
            if (classWorkItem.Status == WorkItemStatus.NotRunning)
            {
                classWorkItem.Run();
            }
        }

        private static IWorkItem CreateWorkItem(Type classType, IWorkItem parent)
        {
            Type[] types = new Type[] { typeof(IWorkItem) };
            var constructorInfo = classType.GetConstructor(types);

            ParameterExpression param = Expression.Parameter(typeof(IWorkItem), "parent");
            ParameterExpression[] args = new ParameterExpression[1] { param };
            var body = Expression.New(constructorInfo, args);
            var outer = Expression.Lambda<Func<IWorkItem, IWorkItem>>(body, args);

            IWorkItem workItem = outer.Compile()(parent);
            parent.WorkItems.Add(workItem);
            return workItem;
        }
    }
}