    export enum WorkItemStatus
    {
        /// <summary>
        /// The <see cref="IWorkItem"/> is not running.
        /// </summary>
        NotRunning,
        /// <summary>
        /// The <see cref="IWorkItem"/> is running.
        /// </summary>
        Running,
        /// <summary>
        /// The <see cref="IWorkItem"/> is disposed.
        /// </summary>
        Disposed,
    }