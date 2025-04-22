using System.IO;

namespace System.Io
{
    /// <summary>
    /// Provides a wrapper for a <see cref="IO.Stream"/> that contains additional information about where the stream starts and ends as part
    /// of a <see cref="MultiStream"/>.
    /// </summary>
    internal sealed class StreamInfo : IDisposable
    {
        #region Fields

        private bool _isDisposed;

        #endregion

        #region Construction

        /// <summary>  
        /// Creates a new <see cref="StreamInfo"/> instance.  
        /// </summary>  
        /// <param name="stream">The base <see cref="IO.Stream"/> to wrap.</param>  
        /// <param name="start">The start position of the specified <see cref="IO.Stream"/> within the single, merged stream.</param>  
        /// <param name="filename">The name of the file associated with the specified <see cref="IO.Stream"/>.</param>  
        public StreamInfo(Stream stream, long start, string filename)
        {
            Stream = stream;
            Start = start;
            Length = stream.Length;
            End = start + Length - 1;
            Filename = filename;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The end position of the wrapped <see cref="IO.Stream"/> within the <see cref="MultiStream"/>.
        /// </summary>
        public long End { get; private set; }
        /// <summary>
        /// The name of the file the wrapped <see cref="IO.Stream"/> is accessing.
        /// </summary>
        public string Filename { get; private set; }
        /// <summary>
        /// The length of the wrapped <see cref="IO.Stream"/>.
        /// </summary>
        public long Length { get; private set; }
        /// <summary>
        /// The start position of the wrapped <see cref="IO.Stream"/> within the <see cref="MultiStream"/>.
        /// </summary>
        public long Start { get; private set; }
        /// <summary>
        /// The wrapped <see cref="IO.Stream"/>.
        /// </summary>
        public Stream Stream { get; private set; }

        #endregion

        #region Methods

        #region Public

        /// <inheritdoc/>
        public void Dispose()
        {
            if (!_isDisposed)
            {
                Stream.Dispose();

                Start = -1;
                End = -1;
                Length = -1;

                _isDisposed = true;
            }

            GC.SuppressFinalize(this);
        }

        #endregion

        #endregion
    }
}