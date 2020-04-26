using System;
using System.Collections.Generic;
using System.Text;

namespace StateBuilder.Library.Infrastructure
{
    public class BroadCastStateEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StateChangedEventArgs"/> class using the
        /// provided key and new value.
        /// </summary>
        public BroadCastStateEventArgs(string key, object newValue)
        {
            Key = key;
            NewValue = newValue;
        }

        /// <summary>
        /// Gets and sets the changed <see cref="State"/> item key.
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Gets and sets the <see cref="State"/> item's new value.
        /// </summary>
        public object NewValue { get; }
    }
}
