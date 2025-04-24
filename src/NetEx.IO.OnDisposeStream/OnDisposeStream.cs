using System;
using System.IO;

namespace NetEx.IO
{
    /// <summary>
    /// Creates a wrapper around a <see cref="Stream"/> that can be used to perform additional cleanup when the underlying stream is disposed.
    /// </summary>
    public sealed class OnDisposeStream : Stream
    {
        #region Fields

        private readonly Action<bool> _disposeAction;
        private readonly Stream _stream;

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="OnDisposeStream"/> class.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to wrap.</param>
        /// <param name="disposeAction">The <see cref="Action{T}"/> to invoke when the wrapped stream is disposed.</param>
        public OnDisposeStream(Stream stream, Action<bool> disposeAction)
        {
            _stream = stream;
            _disposeAction = disposeAction;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether the wrapped stream supports reading.
        /// </summary>
        /// <value><see langword="true"/> if the wrapped stream supports reading; otherwise, <see langword="false"/>.</value>
        /// <remarks>
        /// <para>If the wrapped stream does not support reading, calls to the <see cref="Read"/>, <see cref="Stream.ReadByte"/>, and <see cref="Stream.BeginRead"/> methods throw a <see cref="NotSupportedException"/>.</para>
        /// <para>If the wrapped stream is closed, this property may return <see langword="false"/>.</para>
        /// <para>Please note that the behaviour described is not guaranteed as it is dependent upon the implementation of the wrapped stream. Refer to the documentation for the wrapped stream implementation for expected behaviour.</para>
        /// </remarks>
        public override bool CanRead => _stream.CanRead;
        /// <summary>
        /// Gets a value indicating whether the wrapped stream supports seeking.
        /// </summary>
        /// <value><see langword="true"/> if the wrapped stream supports seeking; otherwise, <see langword="false"/>.</value>
        /// <remarks>
        /// <para>If the wrapped stream does not support seeking, calls to <see cref="Length"/>, <see cref="SetLength"/>, <see cref="Position"/>, and <see cref="Seek"/> may throw a <see cref="NotSupportedException"/>.</para>
        /// <para>If the wrapped stream is closed, this property may return <see langword="false"/>.</para>
        /// <para>Please note that the behaviour described is not guaranteed as it is dependent upon the implementation of the wrapped stream. Refer to the documentation for the wrapped stream implementation for expected behaviour.</para>
        /// </remarks>
        public override bool CanSeek => _stream.CanSeek;
        /// <summary>
        /// Gets a value indicating whether the wrapped stream supports writing.
        /// </summary>
        /// <value><see langword="true"/> if the wrapped stream supports writing; otherwise, <see langword="false"/>.</value>
        /// <remarks>
        /// <para>If the wrapped stream does not support writing, a call to <see cref="Write"/>, <see cref="Stream.BeginWrite"/>, or <see cref="Stream.WriteByte"/> throws a <see cref="NotSupportedException"/>. In such cases, <see cref="Flush"/> is typically implemented as an empty method to ensure full compatibility with other <see cref="Stream"/> types since it's valid to flush a read-only stream.</para>
        /// <para>If the wrapped stream is closed, this property may return <see langword="false"/>.</para>
        /// <para>Please note that the behaviour described is not guaranteed as it is dependent upon the implementation of the wrapped stream. Refer to the documentation for the wrapped stream implementation for expected behaviour.</para>
        /// </remarks>
        public override bool CanWrite => _stream.CanWrite;
        /// <summary>
        /// Gets the length in bytes of the wrapped stream.
        /// </summary>
        /// <value>A long value representing the length of the wrapped stream in bytes.</value>
        /// <exception cref="NotSupportedException">The wrapped stream does not support seeking and the length is unknown.</exception>
        /// <exception cref="IOException">An I/O error occured.</exception>
        /// <exception cref="ObjectDisposedException">Methods were called after the wrapped stream was closed.</exception>
        /// <remarks>
        /// <para>Please note that the behaviour described is not guaranteed as it is dependent upon the implementation of the wrapped stream. Refer to the documentation for the wrapped stream implementation for expected behaviour.</para>
        /// </remarks>
        public override long Length => _stream.Length;
        /// <summary>
        /// Gets or sets the position within the wrapped stream.
        /// </summary>
        /// <value>The current position within the wrapped stream.</value>
        /// <exception cref="NotSupportedException">The wrapped stream does not support seeking.</exception>
        /// <exception cref="IOException">An I/O error occured.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The position is set to a value outside the supported range of the wrapped stream.</exception>
        /// <exception cref="EndOfStreamException">Attempted seeking past the end of a wrapped stream that does not support this.</exception>
        /// <exception cref="ObjectDisposedException">Methods were called after the wrapped stream was closed.</exception>
        /// <remarks>
        /// <para>The wrapped stream must support seeking to get or set the position. Use the <see cref="CanSeek"/> property to determine whether the wrapped stream supports seeking.</para>
        /// <para>Seeking to any location beyond the length of the wrapped stream may be supported.</para>
        /// <para>Please note that the behaviour described is not guaranteed as it is dependent upon the implementation of the wrapped stream. Refer to the documentation for the wrapped stream implementation for expected behaviour.</para>
        /// </remarks>
        public override long Position
        {
            get => _stream.Position; 
            set => _stream.Position = value;
        }

        #endregion

        #region Methods

        #region Protected

        /// <inheritdoc/>
        protected sealed override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Dispose of the wrapped stream.
                _stream.Dispose();
            }

            // With the wrapped stream disposed, invoke the dispose action.
            _disposeAction(disposing);

            // Finally, call the base Dispose method.
            base.Dispose(disposing);
        }

        #endregion

        #region Public

        /// <summary>
        /// When supported by the wrapped stream, clears all buffers for the stream and causes any buffered data to be written to the underlying device.
        /// </summary>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="ObjectDisposedException">Methods were called after the wrapped stream was closed.</exception>
        /// <remarks>
        /// <para>In a wrapped stream that doesn't support writing, <see cref="Flush"/> is typically implemented as an empty method to ensure full compatibility with other <see cref="Stream"/> types since it's valid to flush a read-only stream.</para>
        /// <para>When using the <see cref="StreamWriter"/> or <see cref="BinaryWriter"/> class, do not flush the base <see cref="Stream"/> object. Instead, use the class's <see cref="Flush"/> or <see cref="Stream.Close"/> method, which makes sure that the data is flushed to the underlying stream first and then written to the file.</para>
        /// <para>Please note that the behaviour described is not guaranteed as it is dependent upon the implementation of the wrapped stream. Refer to the documentation for the wrapped stream implementation for expected behaviour.</para>
        /// </remarks>
        public override void Flush()
        {
            _stream.Flush();
        }
        /// <summary>
        /// Reads a sequence of bytes from the wrapped stream and advances the position within the stream by the number of bytes read.
        /// </summary>
        /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values between <paramref name="offset"/> and (<paramref name="offset"/> + <paramref name="count"/> - 1) replaced by the bytes read from the wrapped stream.</param>
        /// <param name="offset">The zero-based byte offset in <paramref name="buffer"/> at which to begin storing data from the wrapped stream.</param>
        /// <param name="count">The maximum number of bytes to be read from the wrapped stream.</param>
        /// <returns>The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if <c>count</c> is 0 or the end of the stream has been reached.</returns>
        /// <exception cref="ArgumentException">The sum of <paramref name="offset"/> and <paramref name="count"/> is outside the supported range of the wrapped stream.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="offset"/> or <paramref name="count"/> are set to a value outside the supported range of the wrapped stream.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="NotSupportedException">The wrapped stream does not support reading.</exception>
        /// <exception cref="ObjectDisposedException">Methods were called after the wrapped stream was closed.</exception>
        /// <remarks>
        /// <para>Use the <see cref="CanRead"/> property to determine whether the current instance supports reading.</para>
        /// <para>Wrapped streams will read a maximum of <c>count</c> bytes and store them in <c>buffer</c> beginning at <c>offset</c>. The current position within the wrapped stream is advanced by the number of bytes read; however, if an exception occurs, the current position within the wrapped stream remains unchanged. Wrapped streams will return the number of bytes read. If more than zero bytes are requested, the wrapped stream will not complete the operation until at least one byte of data can be read (some streams may similarly not complete until at least one byte is available even if zero bytes were requested, but no data will be consumed from the stream in such a case). <see cref="Read"/> returns 0 only if zero bytes were requested or when there is no more data in the wrapped stream and no more is expected (such as a closed socket or end of file). A wrapped stream may return fewer bytes than requested even if the end of the stream has not been reached.</para>
        /// <para>Use <see cref="BinaryReader"/> for reading primitive data types.</para>
        /// <para>Please note that the behaviour described is not guaranteed as it is dependent upon the implementation of the wrapped stream. Refer to the documentation for the wrapped stream implementation for expected behaviour.</para>
        /// </remarks>
        public override int Read(byte[] buffer, int offset, int count)
        {
            return _stream.Read(buffer, offset, count);
        }
        /// <summary>
        /// Sets the position within the wrapped stream.
        /// </summary>
        /// <param name="offset">A byte offset relative to the <paramref name="origin"/> parameter.</param>
        /// <param name="origin">A value of type <see cref="SeekOrigin"/> indicating the reference point used to obtain the new position.</param>
        /// <returns>The new position within the wrapped stream.</returns>
        /// <exception cref="ArgumentException">There is an invalid <see cref="T:System.IO.SeekOrigin"/>.
        /// <para>-or-</para>
        /// <paramref name="offset"/> is set to a value outside the supported range of the wrapped stream.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="offset"/> is set to a value outside the supported range of the wrapped stream.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="NotSupportedException">The wrapped stream does not support seeking, such as if the stream is constructed from a pipe or console output.</exception>
        /// <exception cref="ObjectDisposedException">Methods were called after the wrapped stream was closed.</exception>
        /// <remarks>
        /// <para>Use the <see cref="CanSeek"/> property to determine whether the wrapped stream supports seeking.</para>
        /// <para>If <c>offset</c> is negative, the new position is required to precede the position specified by <c>origin</c> by the number of bytes specified by <c>offset</c>.If <c>offset</c> is zero(0), the new position is required to be the position specified by <c>origin</c>.If <c>offset</c> is positive, the new position is required to follow the position specified by <c>origin</c> by the number of bytes specified by <c>offset</c>.</para>
        /// <para>Seeking to any location beyond the length of the wrapped stream may be supported.</para>
        /// <para>Please note that the behaviour described is not guaranteed as it is dependent upon the implementation of the wrapped stream. Refer to the documentation for the wrapped stream implementation for expected behaviour.</para>
        /// </remarks>
        public override long Seek(long offset, SeekOrigin origin)
        {
            return _stream.Seek(offset, origin);
        }
        /// <summary>
        /// Sets the length of the wrapped stream
        /// </summary>
        /// <param name="value">The desired length of the wrapped stream in bytes.</param>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="NotSupportedException">The wrapped stream does not support both writing and seeking, such as if the stream is constructed from a pipe or console output.</exception>
        /// <exception cref="ObjectDisposedException">Methods were called after the wrapped stream was closed.</exception>
        /// <remarks>
        /// <para>If the specified value is less than the current length of the stream, the stream is truncated. If the specified value is larger than the current length of the stream, the stream is expanded. If the stream is expanded, the contents of the stream between the old and the new length are not defined.</para>
        /// <para>The wrapped stream must support both writing and seeking for <c>SetLength</c> to work.</para>
        /// <para>Use the <see cref="CanWrite"/> property to determine whether the wrapped supports writing, and the <see cref="CanSeek"/> property to determine whether seeking is supported.</para>
        /// <para>Please note that the behaviour described is not guaranteed as it is dependent upon the implementation of the wrapped stream. Refer to the documentation for the wrapped stream implementation for expected behaviour.</para>
        /// </remarks>
        public override void SetLength(long value)
        {
            _stream.SetLength(value);
        }
        /// <summary>
        /// Writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.
        /// </summary>
        /// <param name="buffer">An array of bytes. This method copies <paramref name="count"/> bytes from <paramref name="buffer"/> to the current stream.</param>
        /// <param name="offset">The zero-based byte offset in <paramref name="buffer"/> at which to begin copying bytes to the current stream.</param>
        /// <param name="count">The number of bytes to be written to the current stream.</param>
        /// <exception cref="ArgumentException">The sum of <paramref name="offset"/> and <paramref name="count"/> sum is outside the supported range of the wrapped stream.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="offset"/> or <paramref name="count"/> are set to a value outside the supported range of the wrapped stream.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="NotSupportedException">The wrapped stream does not support writing.</exception>
        /// <exception cref="ObjectDisposedException">Methods were called after the wrapped stream was closed.</exception>
        /// <remarks>
        /// <para>Use the <see cref="CanWrite"/> property to determine whether the wrapped stream supports writing.</para>
        /// <para>If the write operation is successful, the position within the stream advances by the number of bytes written. If an exception occurs, the position within the stream should remain unchanged.</para>
        /// <para>Please note that the behaviour described is not guaranteed as it is dependent upon the implementation of the wrapped stream. Refer to the documentation for the wrapped stream implementation for expected behaviour.</para>
        /// </remarks>
        public override void Write(byte[] buffer, int offset, int count)
        {
            _stream.Write(buffer, offset, count);
        }

        #endregion

        #endregion
    }
}