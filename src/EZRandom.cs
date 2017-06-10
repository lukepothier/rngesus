using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace EZRNG
{
    public class EZRandom : IDisposable
    {
        bool _disposed;
        SafeHandle _handle = new SafeFileHandle(IntPtr.Zero, true);

        #region GC

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
                _handle.Dispose();

            _disposed = true;
        }

        #endregion GC
    }
}
