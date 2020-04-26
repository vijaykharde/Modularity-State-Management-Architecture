using Microsoft.AspNetCore.SignalR;
using StateBuilder.Library.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure.Web.Extensions
{
    public class NotifyHub : Hub<ITypedHubClient>
    {

    }

    public interface ITypedHubClient
    {
        //Task BroadcastMessage(IWorkItem workItem);
        //Task BroadcastMessage(State workItem);
        //Task BroadcastMessage(object obj);
        Task BroadcastMessage(JSONEntity obj);
    }
}
