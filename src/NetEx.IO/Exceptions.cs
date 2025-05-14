using System;
using System.IO;

namespace NetEx.IO
{
    internal static class Exceptions
    {
        #region Methods

        public static InvalidOperationException MultiStreamDoesNotSupportTimeouts()
        {
            return new InvalidOperationException("MultiStream does not support timeouts.");
        }
        public static ArgumentException InvalidSeekOrigin()
        {
            return new ArgumentException("Invalid seek origin.");
        }
        public static NotSupportedException MultiStreamDoesNotSupportWriting()
        {
            return new NotSupportedException("MultiStream does not support writing.");
        }
        public static EndOfStreamException MultiStreamPositionEndOfStream(long index)
        {
            return new EndOfStreamException("Position must be non-negative and less than or equal to Length.");
        }
        public static ArgumentOutOfRangeException MultiStreamPositionLessThanZero(long index)
        {
            return new ArgumentOutOfRangeException("Position", index, "Position must be non-negative.");
        }
        public static InvalidOperationException MultiStreamStreamDoesNotSupportRead()
        {
            return new InvalidOperationException("MultiStream can only wrap streams that support reading.");
        }
        public static ObjectDisposedException StreamDisposed()
        {
            return new ObjectDisposedException(null, "The stream has been closed.");
        }

        #endregion
    }
}