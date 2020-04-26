using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace StateBuilder.Library.Infrastructure
{
    [JsonArray]
    public class WorkItemCollection : IEnumerable<IWorkItem>
    {
        private readonly List<IWorkItem> _workItems = new List<IWorkItem>();
        private readonly IWorkItem _parent;

        public WorkItemCollection(IWorkItem parent)
        {
            _parent = parent;
        }

        public event EventHandler<IWorkItem> WorkItemAdded;
        public event EventHandler<IWorkItem> WorkItemRemoved;

        public void Add(IWorkItem workItem)
        {
            workItem.Disposed += OnWorkItemDisposed;
            _workItems.Add(workItem);
            WorkItemAdded?.Invoke(this, workItem);
        }

        public TWorkItem AddNew<TWorkItem>()
            where TWorkItem : IWorkItem
        {
            return AddNew<TWorkItem>(Guid.NewGuid());
        }

        public TWorkItem AddNew<TWorkItem>(Guid id)
            where TWorkItem : IWorkItem
        {
            return (TWorkItem)AddNew(typeof(TWorkItem), id);
        }

        public IWorkItem AddNew(Type typeToBuild)
        {
            return AddNew(typeToBuild, Guid.NewGuid());
        }

        public IWorkItem AddNew(Type typeToBuild, Guid id)
        {
            var workItem = (IWorkItem)_parent.Container.Resolve(typeToBuild, null);
            workItem.Id = id;
            Add(workItem);
            return workItem;
        }

        public TWorkItem Get<TWorkItem>()
            where TWorkItem : IWorkItem
        {
            return _workItems.OfType<TWorkItem>().SingleOrDefault();
        }

        public TWorkItem Get<TWorkItem>(Guid id)
            where TWorkItem : IWorkItem
        {
            return _workItems.OfType<TWorkItem>().SingleOrDefault(x => x.Id == id);
        }

        public IWorkItem Get(string id)
        {
            return OfGuid().SingleOrDefault();

            IEnumerable<IWorkItem> OfGuid()
            {
                foreach (var item in _workItems)
                {
                    if (item.Id.ToString() == id)
                    {
                        yield return item;
                    }
                }
            }
        }

        public IWorkItem Get(Type workItemType)
        {
            return OfType().SingleOrDefault();

            IEnumerable<IWorkItem> OfType()
            {
                foreach (var item in _workItems)
                {
                    if (workItemType.IsInstanceOfType(item))
                    {
                        yield return item;
                    }
                }
            }
        }

        public IWorkItem Get(Type workItemType, Guid id)
        {
            return OfType().SingleOrDefault(x => x.Id == id);

            IEnumerable<IWorkItem> OfType()
            {
                foreach (var item in _workItems)
                {
                    if (workItemType.IsInstanceOfType(item))
                    {
                        yield return item;
                    }
                }
            }
        }

        public void Remove(IWorkItem workItem)
        {
            _workItems.Remove(workItem);
            workItem.Disposed -= OnWorkItemDisposed;
            WorkItemRemoved?.Invoke(this, workItem);
        }

        public IEnumerator<IWorkItem> GetEnumerator()
        {
            return _workItems.ToList().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void OnWorkItemDisposed(object sender, EventArgs eventArgs)
        {
            var workItem = (IWorkItem)sender;
            Remove(workItem);
        }
    }
}
