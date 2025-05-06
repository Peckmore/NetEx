namespace NetEx.IO
{
    internal static class Exceptions
    {
        #region Methods

        public static ObjectDisposedException StreamDisposed()
        {
            return new ObjectDisposedException(null, "The stream has been closed.");
        }
        public static InvalidOperationException StreamDoesNotSupportTimeouts()
        {
            return new InvalidOperationException("The stream does not support timeouts.");
        }
        public static NotSupportedException StreamDoesNotSupportWriting()
        {
            return new NotSupportedException("The stream does not support writing.");
        }

        #endregion
    }
}