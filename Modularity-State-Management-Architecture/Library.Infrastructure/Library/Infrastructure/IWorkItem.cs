using Newtonsoft.Json;
using StateBuilder.Events;
using System;
using Unity;

namespace StateBuilder.Library.Infrastructure
{
    [JsonObject(ItemReferenceLoopHandling = ReferenceLoopHandling.Serialize, IsReference = true)]
    public interface IWorkItem : IDisposable
    {
        [JsonIgnore]
        Guid Id { get; set; }
        //[JsonProperty(ReferenceLoopHandling = ReferenceLoopHandling.Ignore, IsReference = true)]
        [JsonIgnore]
        IWorkItem Parent { get; }
        //[JsonIgnore]
        //[JsonProperty(ReferenceLoopHandling = ReferenceLoopHandling.Ignore, IsReference = true)]
        [JsonIgnore]
        IWorkItem RootWorkItem { get; }
        [JsonIgnore]
        IUnityContainer Container { get; }
        [JsonProperty]
        State State { get; }
        [JsonProperty]
        WorkItemStatus Status { get; }

        [JsonIgnore]
        WorkItemCollection WorkItems { get; }

        event EventHandler Disposed;
        event EventHandler<StateChangedEventArgs> StateChanged;

        SubscriptionToken SubscribeStateChangedEvent(string stateName, Action action);
        SubscriptionToken SubscribeStateChangedEvent(string stateName, Action<object> action);
        SubscriptionToken SubscribeStateChangedEvent(string stateName, Action<StateChangedEventArgs> action);
        SubscriptionToken SubscribeStateChangedEvent<T>(string stateName, Action<T> action);
        SubscriptionToken SubscribeStateChangedEvent<T>(string stateName, Action<StateChangedEventArgs<T>> action);

        void Activate();
        void Run(Action continueWith = null);
        Type GetTypeOfState(string stateName);

        event EventHandler<BroadCastStateEventArgs> BroadCastEvent;
        void BroadCastState(object sender, BroadCastStateEventArgs eventArgs);
    }
    public static class WorkItemExtensions
    {
        private class __WorkItem : WorkItem
        {
            public __WorkItem(IUnityContainer container)
                : base(container)
            {
            }
        }
        private static IWorkItem _rootWorkItem = null;
        public static IWorkItem RootWorkItem
        {
            get
            {
                if (_rootWorkItem == null)
                {
                    UnityContainer container = new UnityContainer();
                    _rootWorkItem = new __WorkItem(container);
                }
                return _rootWorkItem;
            }
        }
    }
}
