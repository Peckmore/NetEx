using System;

namespace NetEx.IO
{
    internal static class Exceptions
    {
        #region Methods

        public static InvalidOperationException MultiStreamDoesNotSupportTimeouts()
        {
            return new InvalidOperationException("MultiStream does not support timeouts.");
        }
        public static NotSupportedException MultiStreamDoesNotSupportWriting()
        {
            return new NotSupportedException("MultiStream does not support writing.");
        }
        public static ObjectDisposedException StreamDisposed()
        {
            return new ObjectDisposedException(null, "The stream has been closed.");
        }

        #endregion
    }
}