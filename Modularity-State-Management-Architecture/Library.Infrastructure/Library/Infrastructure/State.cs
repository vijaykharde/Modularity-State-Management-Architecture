using Newtonsoft.Json;
using StateBuilder.Guard;
using StateBuilder.Threading;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace StateBuilder.Library.Infrastructure
{
    [JsonObject]
    [Serializable]
    public class State : IEnumerable<KeyValuePair<string, object>>, IDisposable
    {
        private readonly List<(State State, Dictionary<string, string> Mappings, Dictionary<string, string> ReverseMappings)> _sources = new List<(State, Dictionary<string, string>, Dictionary<string, string>)>();
        private readonly Dictionary<string, object> _values = new Dictionary<string, object>();

        /// <summary>
        /// Initializes a new instance of the <see cref="State"/> class using a random ID.
        /// </summary>
        public State()
        {
            Id = Guid.NewGuid().ToString();
        }
        [JsonProperty]
        public Dictionary<string, object> States
        {
            get
            {
                return _values;
            }
        }
        [JsonProperty]
        /// <summary>
        /// Gets the state ID.
        /// </summary>
        public string Id { get; }

        [JsonIgnore]
        /// <summary>
        /// Used to synchronize access to this <see cref="State"/>.
        /// </summary>
        public ReaderWriterLockSlim SyncRoot { get; set; } = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

        /// <summary>
        /// This event is raised when the state has changed.
        /// </summary>
        public event EventHandler<StateChangedEventArgs> StateChanged;

        /// <summary>
        /// Gets and sets values on the state.
        /// </summary>
        /// <remarks>
        /// Derived classes must use this accessor in order to get the <see cref="StateChanged"/> fired. 
        /// This is made protected to force that pattern for strongly typed models, and forbid access to 
        /// the generic dictionary in those cases (unless the developer explicitly exposes a public 
        /// indexer as the <see cref="State"/> class does).
        /// </remarks>
        [JsonProperty]
        public object this[string key]
        {
            get
            {
                Argument.NotNull(key, "key");

                using (new ReaderLock(SyncRoot))
                {
                    _values.TryGetValue(key, out var value);
                    if (value == null)
                    {
                        foreach (var source in _sources)
                        {
                            if (source.ReverseMappings == null)
                            {
                                value = source.State[key];
                            }
                            else if (source.ReverseMappings.TryGetValue(key, out var newKey))
                            {
                                value = source.State[newKey];
                            }
                            if (value != null)
                            {
                                break;
                            }
                        }
                    }
                    return value;
                }
            }
            set
            {
                Argument.NotNull(key, "key");

                object oldvalue;
                using (new WriterLock(SyncRoot))
                {
                    _values.TryGetValue(key, out oldvalue);
                    if (value != null)
                    {
                        _values[key] = value;
                    }
                    else
                    {
                        _values.Remove(key);
                    }
                }

                RaiseStateChanged(key, value, oldvalue);
            }
        }

        public void AttachSource(State state, Dictionary<string, string> mappings = null)
        {
            state.StateChanged += OnSourceStateChanged;
            using (new WriterLock(SyncRoot))
            {
                _sources.Add((state, mappings, mappings?.ToDictionary(x => x.Value, x => x.Key)));
            }
        }

        public void Load(IEnumerable<KeyValuePair<string, object>> values)
        {
            foreach (var value in values)
            {
                this[value.Key] = value.Value;
            }
        }

        public void Clear()
        {
            IDictionary<string, object> values;
            using (new WriterLock(SyncRoot))
            {
                values = new Dictionary<string, object>(_values);
                _values.Clear();
            }
            foreach (var entry in values)
            {
                RaiseStateChanged(entry.Key, null, entry.Value);
            }
        }

        /// <summary>
        /// Removes the item with the given key from the state.
        /// </summary>
        public bool Remove(string key)
        {
            Argument.NotNull(key, "key");

            object oldvalue;
            using (new WriterLock(SyncRoot))
            {
                _values.TryGetValue(key, out oldvalue);
                if (!_values.Remove(key))
                {
                    return false;
                }
            }

            RaiseStateChanged(key, null, oldvalue);
            return true;
        }

        public void Dispose()
        {
            StateChanged = null;

            foreach (var source in _sources)
            {
                source.State.StateChanged -= OnSourceStateChanged;
            }
            _sources.Clear();
        }

        /// <summary>
        /// Raises the <see cref="StateChanged"/> event for the given state key 
        /// (can be null if it is unknown).
        /// </summary>
        public void RaiseStateChanged(string key, object newValue, object oldValue)
        {
            StateChanged?.Invoke(this, new StateChangedEventArgs(key, newValue, oldValue));
        }

        /// <summary>
        /// Raises the <see cref="StateChanged"/> event for the given state key 
        /// (can be null if it is unknown).
        /// </summary>
        public void RaiseStateChanged(string key, object newValue)
        {
            RaiseStateChanged(key, newValue, null);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            using (new ReaderLock(SyncRoot))
            {
                foreach (var value in _values)
                {
                    yield return value;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void OnSourceStateChanged(object sender, StateChangedEventArgs eventArgs)
        {
            object value;
            using (new ReaderLock(SyncRoot))
            {
                _values.TryGetValue(eventArgs.Key, out value);
            }
            if (value == null)
            {
                var source = _sources.Single(x => ReferenceEquals(sender, x.State));
                if (source.Mappings == null)
                {
                    RaiseStateChanged(eventArgs.Key, eventArgs.NewValue, eventArgs.OldValue);
                }
                else if (source.Mappings.TryGetValue(eventArgs.Key, out var newKey))
                {
                    RaiseStateChanged(newKey, eventArgs.NewValue, eventArgs.OldValue);
                }
            }
        }
    }
}
