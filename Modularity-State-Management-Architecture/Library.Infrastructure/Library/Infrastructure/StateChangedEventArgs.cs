using System;

namespace StateBuilder.Library.Infrastructure
{
    /// <summary>
    /// Provides data for a StateChanged event.
    /// </summary>
    public class StateChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StateChangedEventArgs"/> class using the
        /// provided key and new value.
        /// </summary>
        public StateChangedEventArgs(string key, object newValue)
        {
            Key = key;
            NewValue = newValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StateChangedEventArgs"/> class using the
        /// provided key, new value, and old value.
        /// </summary>
        public StateChangedEventArgs(string key, object newValue, object oldValue)
        {
            Key = key;
            NewValue = newValue;
            OldValue = oldValue;
        }

        /// <summary>
        /// Gets and sets the changed <see cref="State"/> item key.
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Gets and sets the <see cref="State"/> item's new value.
        /// </summary>
        public object NewValue { get; }

        /// <summary>
        /// Gets and sets the <see cref="State"/> item's old value.
        /// </summary>
        public object OldValue { get; }
    }

    /// <summary>
    /// Provides data for a StateChanged event.
    /// </summary>
    public class StateChangedEventArgs<T> : StateChangedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StateChangedEventArgs"/> class using the
        /// provided key and new value.
        /// </summary>
        public StateChangedEventArgs(string key, T newValue)
            : base(key, newValue)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StateChangedEventArgs"/> class using the
        /// provided key, new value, and old value.
        /// </summary>
        public StateChangedEventArgs(string key, T newValue, T oldValue)
            : base(key, newValue, oldValue)
        {
        }

        /// <summary>
        /// Gets and sets the <see cref="State"/> item's new value.
        /// </summary>
        public new T NewValue => (T)base.NewValue;

        /// <summary>
        /// Gets and sets the <see cref="State"/> item's old value.
        /// </summary>
        public new T OldValue => (T)base.OldValue;
    }
}
