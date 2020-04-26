using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using StateBuilder.Library.Infrastructure;
using Unity;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Library.Infrastructure.Web.Extensions
{
    public static class WorkItemExtensions
    {
        public static void RegisterStateChangedEventForBroadcast(this IWorkItem workItem)
        {
            workItem.BroadCastEvent -= WorkItem_BroadCastEvent;
            workItem.BroadCastEvent += WorkItem_BroadCastEvent;
            //workItem.StateChanged -= WorkItem_StateChanged;
            //workItem.StateChanged += WorkItem_StateChanged;
        }

        private static void WorkItem_StateChanged(object sender, StateChangedEventArgs e)
        {
            if (((IWorkItem)sender).RootWorkItem.Container.IsRegistered<IHttpContextAccessor>())
            {
#if DEBUG
                Debug.WriteLine($"****************WorkItem_StateChanged*****************\n{e.Key}\n****************************");
#endif
                IHttpContextAccessor httpContextAccessor = ((IWorkItem)sender).RootWorkItem.Container.Resolve<IHttpContextAccessor>();
                IHubContext<NotifyHub, ITypedHubClient> hubContext = httpContextAccessor.HttpContext?.RequestServices?.GetService(typeof(IHubContext<NotifyHub, ITypedHubClient>)) as IHubContext<NotifyHub, ITypedHubClient>;
                hubContext?.Clients?.All.BroadcastMessage(new JSONEntity { WorkItemId = ((IWorkItem)sender).Id.ToString(), StateName = e.Key, StateValue = e.NewValue });
            }
        }

        private static void WorkItem_BroadCastEvent(object sender, BroadCastStateEventArgs e)
        {
            ((IWorkItem)sender).BroadCastEvent -= WorkItem_BroadCastEvent;
#if DEBUG
            Debug.WriteLine($"****************WorkItem_BroadCastEvent*****************\n{e.Key}\n****************************");
#endif
            if (((IWorkItem)sender).RootWorkItem.Container.IsRegistered<IHttpContextAccessor>())
            {
                IHttpContextAccessor httpContextAccessor = ((IWorkItem)sender).RootWorkItem.Container.Resolve<IHttpContextAccessor>();
                IHubContext<NotifyHub, ITypedHubClient> hubContext = httpContextAccessor.HttpContext?.RequestServices?.GetService(typeof(IHubContext<NotifyHub, ITypedHubClient>)) as IHubContext<NotifyHub, ITypedHubClient>;
                hubContext?.Clients?.All.BroadcastMessage(new JSONEntity { WorkItemId = ((IWorkItem)sender).Id.ToString(), StateName = e.Key, StateValue = e.NewValue });
            }
        }
    }
}