using System;

namespace StateBuilder.Threading.Synchronization
{
    public abstract class LockBase : IDisposable
    {
        private readonly bool _timedOut;
        private readonly Action _releaseMethod;

        protected LockBase(Action acquireMethod, Action releaseMethod)
        {
            _releaseMethod = releaseMethod;
            acquireMethod();
            _timedOut = false;
        }

        protected LockBase(Func<int, bool> acquireMethod, Action releaseMethod, int timeout)
        {
            _releaseMethod = releaseMethod;
            _timedOut = !acquireMethod(timeout);
        }

        protected LockBase(Func<TimeSpan, bool> acquireMethod, Action releaseMethod, TimeSpan timeout)
        {
            _releaseMethod = releaseMethod;
            _timedOut = !acquireMethod(timeout);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_timedOut)
            {
                _releaseMethod();
            }
        }
    }
}
