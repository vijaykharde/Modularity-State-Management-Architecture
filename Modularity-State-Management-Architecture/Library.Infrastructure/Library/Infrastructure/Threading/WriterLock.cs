using StateBuilder.Threading.Synchronization;
using System;
using System.Threading;

namespace StateBuilder.Threading
{
    public sealed class WriterLock : LockBase
    {
        public WriterLock(ReaderWriterLockSlim readerWriterLock)
            : base(readerWriterLock.EnterWriteLock, readerWriterLock.ExitWriteLock)
        {
        }

        public WriterLock(ReaderWriterLockSlim readerWriterLock, int timeout)
            : base(readerWriterLock.TryEnterWriteLock, readerWriterLock.ExitWriteLock, timeout)
        {
        }

        public WriterLock(ReaderWriterLockSlim readerWriterLock, TimeSpan timeout)
            : base(readerWriterLock.TryEnterWriteLock, readerWriterLock.ExitWriteLock, timeout)
        {
        }
    }
}
