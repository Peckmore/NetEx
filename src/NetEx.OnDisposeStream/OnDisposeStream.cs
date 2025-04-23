namespace System.IO
{
    /// <summary>
    /// Creates a stream wrapper that can be used to perform additional cleanup when the underlying stream is disposed.
    /// </summary>
    public class OnDisposeStream : Stream
    {
        #region Fields

        private readonly Action<bool> _disposeAction;
        private readonly Stream _stream;

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="OnDisposeStream"/> class.
        /// </summary>
        /// <param name="stream">The <see cref=Stream" /> to wrap.</param>
        /// <param name="disposeAction">The <see cref="Action" /> to invoke when the wrapped stream is disposed.</param>
        public OnDisposeStream(Stream stream, Action<bool> disposeAction)
        {
            // Set our variables.
            _stream = stream;
            _disposeAction = disposeAction;
        }

        #endregion

        #region Properties

        /// <summary>Gets a value indicating whether the wrapped stream supports reading.</summary>
        /// <value><see langword="true" /> if the stream supports reading; otherwise, <see langword="false" />.</value>
        /// <remarks>
        /// <para>If the underlying stream does not support reading, calls to the <see cref="Read"/> and <see cref="ReadByte"/> methods throw a <see cref="NotSupportedException"/>.</para>
        /// <para>If the stream is closed, this property returns <see langword="false"/>.</para>
        /// </remarks>
        public override bool CanRead => _stream.CanRead;
        /// <summary>Gets a value indicating whether the wrapped stream supports seeking.</summary>
        /// <value><see langword="true" /> if the stream is open.</value>
        /// <remarks>
        /// <para>If the underlying stream does not support seeking, calls to <see cref="Length"/>, <see cref="SetLength"/>, <see cref="Position"/>, and <see cref="Seek"/> throw a <see cref="NotSupportedException"/>.</para>
        /// <para>If the stream is closed, this property returns <see langword="false"/>.</para>
        /// </remarks>
        public override bool CanSeek => _stream.CanSeek;
        /// <summary>Gets a value indicating whether the wrapped stream supports writing.</summary>
        /// <value><see langword="true" /> if the stream supports writing; otherwise, <see langword="false" />.</value>
        /// <remarks>
        /// <para>If the underlying stream does not support reading, a call to <see cref="SetLength"/>, <see cref="Write"/>, or <see cref="WriteByte"/> throws a <see cref="NotSupportedException"/>.</para>
        /// <para>If the stream is closed, this property returns <see langword="false"/>.</para>
        /// </remarks>
        public override bool CanWrite => _stream.CanWrite;
        /// <summary>Gets the length of the wrapped stream in bytes.</summary>
        /// <value>The length of the stream in bytes.</value>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        public override long Length => _stream.Length;
        /// <inheritdoc />
        public override long Position
        {
            get { return _stream.Position; }
            set { _stream.Position = value; }
        }

        #endregion

        #region Methods

        #region Protected

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            _disposeAction(disposing);

            if (disposing)
            {
                // Dispose of the stream.
                _stream.Dispose();
            }
            
            base.Dispose(disposing);
        }

        #endregion

        #region Public

        /// <inheritdoc />
        public override void Flush()
        {
            _stream.Flush();
        }
        /// <inheritdoc />
        public override int Read(byte[] buffer, int offset, int count)
        {
            return _stream.Read(buffer, offset, count);
        }
        /// <inheritdoc />
        public override long Seek(long offset, SeekOrigin origin)
        {
            return _stream.Seek(offset, origin);
        }
        /// <inheritdoc />
        public override void SetLength(long value)
        {
            _stream.SetLength(value);
        }
        /// <inheritdoc />
        public override void Write(byte[] buffer, int offset, int count)
        {
            _stream.Write(buffer, offset, count);
        }

        #endregion

        #endregion
    }
}