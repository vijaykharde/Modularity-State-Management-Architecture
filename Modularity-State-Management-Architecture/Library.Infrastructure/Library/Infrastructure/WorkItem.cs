using StateBuilder.Events;
using System;
using Unity;
using Unity.Lifetime;

namespace StateBuilder.Library.Infrastructure
{
    public class WorkItem : IWorkItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IWorkItem"/> class.
        /// </summary>
        public WorkItem(IWorkItem parent)
            : this(parent?.Container.CreateChildContainer())
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IWorkItem"/> class.
        /// </summary>
        protected WorkItem(IUnityContainer container)
            : this(container, new State())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IWorkItem"/> class.
        /// </summary>
        private WorkItem(IUnityContainer container, State state)
        {
            Id = Guid.NewGuid();
            Container = container;
            Container?.RegisterInstance<IWorkItem>(this, new ExternallyControlledLifetimeManager());
            State = state;
            WorkItems = new WorkItemCollection(this);
        }

        /// <summary>
        /// Occurs when the <see cref="IWorkItem"/> is terminated.
        /// </summary>
        public event EventHandler Disposed;

        /// <summary>
        /// Gets the root <see cref="IWorkItem"/> (the one at the top of the hierarchy).
        /// </summary>
        public IWorkItem RootWorkItem
        {
            get
            {
                var result = (IWorkItem)this;
                while (result.Parent != null)
                {
                    result = result.Parent;
                }
                return result;
            }
        }

        /// <summary>
        /// Gets/sets the Id of this <see cref="IWorkItem"/>.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets the parent <see cref="IWorkItem"/>.
        /// </summary>
        public IWorkItem Parent { get; }

        /// <summary>
        /// Gets the <see cref="IUnityContainer"/> associated with this <see cref="IWorkItem"/>.
        /// </summary>
        public IUnityContainer Container { get; private set; }

        /// <summary>
        /// Gets the <see cref="State"/> associated with this <see cref="IWorkItem"/>.
        /// </summary>
        public State State { get; }

        /// <summary>
        /// Gets the current <see cref="WorkItemStatus"/> of the <see cref="IWorkItem"/>.
        /// </summary>
        public WorkItemStatus Status { get; private set; } = WorkItemStatus.NotRunning;

        /// <summary>
        /// Returns a collection describing the child <see cref="IWorkItem"/> objects in this WorkItem.
        /// </summary>
        public WorkItemCollection WorkItems { get; }

        /// <summary>
        /// This event is raised when the state has changed.
        /// </summary>
        public event EventHandler<StateChangedEventArgs> StateChanged;
        public event EventHandler<BroadCastStateEventArgs> BroadCastEvent;

        /// <summary>
        /// Disposes the <see cref="IWorkItem"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public SubscriptionToken SubscribeStateChangedEvent(string stateName, Action action)
        {
            return SubscribeStateChangedEvent(stateName, x => action());
        }

        public SubscriptionToken SubscribeStateChangedEvent(string stateName, Action<object> action)
        {
            return SubscribeStateChangedEvent(stateName, x => action(x.NewValue));
        }

        public SubscriptionToken SubscribeStateChangedEvent(string stateName, Action<StateChangedEventArgs> action)
        {
            var stateObject = State[stateName];
            if (stateObject != null)
            {
                action(new StateChangedEventArgs(stateName, stateObject));
            }

            var eventHandler = new EventHandler<StateChangedEventArgs>(OnStateChanged);
            StateChanged += eventHandler;
            return new SubscriptionToken(delegate
            {
                StateChanged -= eventHandler;
            });

            void OnStateChanged(object sender, StateChangedEventArgs eventArgs)
            {
                if (eventArgs.Key == stateName)
                {
                    action(eventArgs);
                }
            }
        }
        
        public SubscriptionToken SubscribeStateChangedEvent<T>(string stateName, Action<T> action)
        {
            return SubscribeStateChangedEvent<T>(stateName, x => action(x.NewValue));
        }

        public SubscriptionToken SubscribeStateChangedEvent<T>(string stateName, Action<StateChangedEventArgs<T>> action)
        {
            var stateObject = (T)State[stateName];
            if (stateObject != null)
            {
                action(new StateChangedEventArgs<T>(stateName, stateObject));
            }

            var eventHandler = new EventHandler<StateChangedEventArgs>(OnStateChanged);
            StateChanged += eventHandler;
            return new SubscriptionToken(delegate
            {
                StateChanged -= eventHandler;
            });

            void OnStateChanged(object sender, StateChangedEventArgs eventArgs)
            {
                if (eventArgs.Key == stateName)
                {
                    action(new StateChangedEventArgs<T>(eventArgs.Key, (T)eventArgs.NewValue, (T)eventArgs.OldValue));
                }
            }
        }

        /// <summary>
        /// Runs the work item by calling the <see cref="OnRunStarted"/> method.
        /// </summary>
        public void Run(Action continueWith = null)
        {
            if (Status != WorkItemStatus.NotRunning)
            {
                throw new InvalidOperationException("Run can only be called when a WorkItem is not running!");
            }
            Status = WorkItemStatus.Running;
            OnRunStarted();
            continueWith?.Invoke();
        }

        /// <summary>
        /// Activates the work item.
        /// </summary>
        public virtual void Activate()
        {
        }

        /// <summary>
        /// Derived classes can override this   method to place custom business logic to execute when the <see cref="Run"/> method is called on the <see cref="IWorkItem"/>.
        /// </summary>
        protected virtual void OnRunStarted()
        {
            State.StateChanged += OnStateChanged;
        }

        public void BroadCastState(object sender, BroadCastStateEventArgs eventArgs)
        {
            BroadCastEvent?.Invoke(sender, eventArgs);
        }
        private void OnStateChanged(object sender, StateChangedEventArgs eventArgs)
        {
            StateChanged?.Invoke(this, eventArgs);
            OnStateChanged(eventArgs);
        }

        protected virtual void OnStateChanged(StateChangedEventArgs eventArgs)
        {
        }

        protected virtual void Dispose(bool disposing)
        {
            Status = WorkItemStatus.Disposed;
            try
            {
                if (disposing)
                {
                    foreach (var workItem in WorkItems)
                    {
                        workItem.Dispose();
                    }

                    State?.Dispose();

                    if (Container != null)
                    {
                        var container = Container;
                        Container = null;
                        container?.Dispose();
                    }
                }
            }
            finally
            {
                Disposed?.Invoke(this, EventArgs.Empty);
            }
        }

        public virtual Type GetTypeOfState(string stateName)
        {
            Type wiStateType = this.State[stateName].GetType();
            return wiStateType;
        }
    }
}
