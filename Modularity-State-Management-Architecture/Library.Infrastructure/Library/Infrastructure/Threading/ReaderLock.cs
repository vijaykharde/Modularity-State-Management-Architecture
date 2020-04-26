using StateBuilder.Threading.Synchronization;
using System;
using System.Threading;

namespace StateBuilder.Threading
{
    public sealed class ReaderLock : LockBase
    {
        public ReaderLock(ReaderWriterLockSlim readerWriterLock)
            : base(readerWriterLock.EnterReadLock, readerWriterLock.ExitReadLock)
        {
        }

        public ReaderLock(ReaderWriterLockSlim readerWriterLock, int timeout)
            : base(readerWriterLock.TryEnterReadLock, readerWriterLock.ExitReadLock, timeout)
        {
        }

        public ReaderLock(ReaderWriterLockSlim readerWriterLock, TimeSpan timeout)
            : base(readerWriterLock.TryEnterReadLock, readerWriterLock.ExitReadLock, timeout)
        {
        }
    }
}
