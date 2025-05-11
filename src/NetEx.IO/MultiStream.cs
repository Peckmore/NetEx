using System;
using System.Collections.Generic;
using System.IO;

namespace NetEx.IO
{
    /// <summary>
    /// Creates a wrapper around multiple <see cref="Stream"/> instances, and presents them as a single, read-only stream.
    /// </summary>
    public class MultiStream : Stream
    {
        #region Fields

        private StreamInfo? _activeStream;
        private readonly List<StreamInfo> _fileStreams;
        private bool _isDisposed;
        private readonly long _length;
        private long _position;

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the wrapped stream currently being read from changes.
        /// </summary>
        public event EventHandler? ActiveStreamNameChanged;

        #endregion

        #region Construction

        /// <summary>
        /// Creates a new <see cref="MultiStream"/> instance.
        /// </summary>
        /// <param name="streams">The collection of <see cref="Stream"/> instances to wrap.</param>
        public MultiStream(IEnumerable<Stream> streams)
        {
            // We need to create a list for where we'll store our streams.
            _fileStreams = new List<StreamInfo>();

            foreach (var stream in streams)
            {
                // MultiStream can only work with streams that support reading and seeking, so we need to check each stream first.
                if (!stream.CanRead || !stream.CanSeek)
                {
                    // If the stream is not readable or seekable then we can't use it, so throw an exception.
                    throw Exceptions.MultiStreamStreamDoesNotSupportRead();
                }

                // Add the stream to our list.
                _fileStreams.Add(new StreamInfo(stream, _length));

                // Increment our total length.
                _length += stream.Length;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// The name of the <see cref="Stream"/> currently being accessed within the <see cref="MultiStream"/>, based on <see cref="Position"/>.
        /// </summary>
        /// <returns>The name of the <see cref="Stream"/> currently being accessed within the <see cref="MultiStream"/>.</returns>
        /// <exception cref="ObjectDisposedException">Methods were called after the stream was closed.</exception>
        public string? ActiveStreamName
        {
            get
            {
                // Check we haven't been diposed.
                if (_isDisposed)
                {
                    // We have, so throw an exception.
                    throw Exceptions.StreamDisposed();
                }

                // Return the `Name` property of the active stream.
                return _activeStream?.Name;
            }
        }
        /// <summary>
        /// Gets a value indicating whether the stream supports reading.
        /// </summary>
        /// <value><see langword="true"/> if the stream supports reading; otherwise, <see langword="false"/>.</value>
        /// <remarks>
        /// <para>If the stream is closed, this property will return <see langword="false"/>.</para>
        /// </remarks>
        public sealed override bool CanRead => !_isDisposed;
        /// <summary>
        /// Gets a value indicating whether the stream supports seeking.
        /// </summary>
        /// <value><see langword="true"/> if the stream supports seeking; otherwise, <see langword="false"/>.</value>
        /// <remarks>
        /// <para>If the stream is closed, this property will return <see langword="false"/>.</para>
        /// </remarks>
        public sealed override bool CanSeek => !_isDisposed;
        /// <summary>
        /// Gets a value indicating whether the stream can time out.
        /// </summary>
        /// <value><see cref="MultiStream"/> does not support streams that can time out, so this property will always return <see langword="false"/>.</value>
        public sealed override bool CanTimeout => false;
        /// <summary>
        /// Gets a value indicating whether the stream supports writing.
        /// </summary>
        /// <value><see cref="MultiStream"/> does not support writing, so this property will always return <see langword="false"/>.</value>
        public sealed override bool CanWrite => false;
        /// <summary>
        /// Gets the length in bytes of the stream.
        /// </summary>
        /// <value>A long value representing the length of the stream in bytes.</value>
        /// <exception cref="ObjectDisposedException">Methods were called after the stream was closed.</exception>
        public sealed override long Length
        {
            get
            {
                // Check we haven't been diposed.
                if (_isDisposed)
                {
                    // We have, so throw an exception.
                    throw Exceptions.StreamDisposed();
                }

                // Return our total length, which is the sum of all the stream lengths.
                return _length;
            }
        }
        /// <summary>
        /// Gets or sets the position within the stream.
        /// </summary>
        /// <value>The current position within the stream.</value>
        /// <exception cref="ArgumentOutOfRangeException">Attempted to set the position to a negative value.</exception>
        /// <exception cref="EndOfStreamException">Attempted seeking past the end of the stream.</exception>
        /// <exception cref="ObjectDisposedException">Methods were called after the stream was closed.</exception>
        public sealed override long Position
        {
            get
            {
                // Check we haven't been diposed.
                if (_isDisposed)
                {
                    // We have, so throw an exception.
                    throw Exceptions.StreamDisposed();
                }

                // Return our position within our collection of wrapped streams.
                return _position;
            }
            set
            {
                // Check that position is not being set to a negative value.
                if (value < 0)
                { 
                    throw Exceptions.MultiStreamPositionLessThanZero(value);
                }
                
                // Check that position is not being set to a value beyond the end of the stream.
                if (value > _length - 1)
                {
                    throw Exceptions.MultiStreamPositionEndOfStream(value);
                }

                _position = value;
            }
        }
        /// <summary>
        /// Gets or sets a value, in milliseconds, that determines how long the stream will attempt to read before timing out.
        /// </summary>
        /// <value>A value, in milliseconds, that determines how long the stream will attempt to read before timing out.</value>
        /// <exception cref="InvalidOperationException">The <see cref="ReadTimeout"/> method always throws an <see cref="InvalidOperationException"/>.</exception>
        public sealed override int ReadTimeout
        {
            get
            {
                // https://learn.microsoft.com/en-us/dotnet/api/system.io.stream.readtimeout?view=net-8.0#notes-to-inheritors
                // "The ReadTimeout property should be overridden to provide the appropriate behavior for the stream. If the stream
                // does not support timing out, this property should raise an InvalidOperationException."
                throw Exceptions.MultiStreamDoesNotSupportTimeouts();
            }
        }
        /// <summary>
        /// Gets or sets a value, in milliseconds, that determines how long the stream will attempt to write before timing out.
        /// </summary>
        /// <value>A value, in milliseconds, that determines how long the stream will attempt to write before timing out.</value>
        /// <exception cref="InvalidOperationException">The <see cref="WriteTimeout"/> method always throws an <see cref="InvalidOperationException"/>.</exception>
        public sealed override int WriteTimeout
        {
            get
            {
                // https://learn.microsoft.com/en-us/dotnet/api/system.io.stream.writetimeout?view=net-8.0#notes-to-inheritors
                // "The WriteTimeout property should be overridden to provide the appropriate behavior for the stream. If the stream
                // does not support timing out, this property should raise an InvalidOperationException."
                throw Exceptions.MultiStreamDoesNotSupportTimeouts();
            }
        }

        #endregion

        #region Methods

        #region Private

        private StreamInfo? GetStream()
        {
            // Grabbing the correct stream is done every Read, so we'll try and do a mini-optimisation and cache the currently active
            // stream, which prevents having to iterate the list every request. Only if we're outside of the active stream will we
            // then have to iterate the list. Possibly this could be improved further by moving this onto the Position property itself
            // in the future?

            // Check whether the current position is within the currently active stream (if not null). If the position falls within
            // the active stream then we don't need to do anything.
            if (!(Position >= _activeStream?.Start && Position <= _activeStream?.End))
            {
                // If we're in here then the current position is outside of the currently active stream (or the stream is null), so we
                // need to figure out which stream we need.

                // Set the active stream to null in case the position requested is outside of all streams.
                _activeStream = null;

                // We'll loop through every stream until we find the one that contains the requested position.
                foreach (var stream in _fileStreams)
                {
                    // Check whether the position is within this stream (stream.Start and stream.End represent the start and end point
                    // of a stream within the single stream, so we can check against them).
                    if (Position <= stream.End)
                    {
                        // We've found the correct stream, so set it to our active stream.
                        _activeStream = stream;

                        // Raise the event to say we have changed stream, and therefore stream name (if supported).
                        ActiveStreamNameChanged?.Invoke(this, EventArgs.Empty);

                        // Break out the loop as we don't need to continue searching.
                        break;
                    }
                }
            }

            // Return the active stream, which is either a stream, or null.
            return _activeStream;
        }

        #endregion

        #region Protected

        /// <inheritdoc/>
        protected sealed override void Dispose(bool disposing)
        {
            // Check we aren't already disposed.
            if (!_isDisposed)
            {
                // Call the OnDispose() method in any derived classes.
                OnDispose(disposing);
                
                // Close each of our file streams.
                foreach (var fileStream in _fileStreams)
                {
                    fileStream.Dispose();
                }
                _fileStreams.Clear();

                _activeStream = null;

                // Set our flag to indicate the object has been disposed.
                _isDisposed = true;
            }

            // Call the base Dispose() method.
            base.Dispose(disposing);
        }
        /// <summary>
        /// Allows derived classes to dispose of any resources when the class is disposed or finalized.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> to indicate that this method has been called during disposal, or
        /// <see langword="false"/> to indicate the method has been called during finalization.</param>
        protected virtual void OnDispose(bool disposing)
        { }

        #endregion

        #region Public

        /// <summary>
        /// Overrides the <see cref="Stream.Flush"/> method so that no action is performed.
        /// </summary>
        /// <remarks>
        /// <para>This method overrides the Stream.Flush method.</para>
        /// <para>Because MultiStream objects are read-only, this method is redundant.</para>
        /// </remarks>
        public override void Flush()
        {
            // https://learn.microsoft.com/en-us/dotnet/api/system.io.stream.flush?view=net-8.0#remarks
            // "In a class derived from Stream that doesn't support writing, Flush is typically implemented as an empty method to
            // ensure full compatibility with other Stream types since it's valid to flush a read-only stream."
        }
        /// <summary>
        /// Reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.
        /// </summary>
        /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values between <paramref name="offset"/> and (<paramref name="offset"/> + <paramref name="count"/> - 1) replaced by the bytes read from the stream.</param>
        /// <param name="offset">The zero-based byte offset in <paramref name="buffer"/> at which to begin storing data from the stream.</param>
        /// <param name="count">The maximum number of bytes to be read from the stream.</param>
        /// <returns>The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if <c>count</c> is 0 or the end of the stream has been reached.</returns>
        /// <exception cref="ArgumentException">The sum of <paramref name="offset"/> and <paramref name="count"/> is outside the supported range of the stream.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="offset"/> or <paramref name="count"/> are set to a value outside the supported range of the stream.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="ObjectDisposedException">Methods were called after the stream was closed.</exception>
        /// <remarks>
        /// <para>MultiStreams will read a maximum of <c>count</c> bytes and store them in <c>buffer</c> beginning at <c>offset</c>. The current position within the stream is advanced by the number of bytes read; however, if an exception occurs, the current position within the stream remains unchanged. MultiStreams will return the number of bytes read. If more than zero bytes are requested, the stream will not complete the operation until at least one byte of data can be read (some streams may similarly not complete until at least one byte is available even if zero bytes were requested, but no data will be consumed from the stream in such a case). <see cref="Read"/> returns 0 only if zero bytes were requested or when there is no more data in the stream and no more is expected (such as a closed socket or end of file). A stream may return fewer bytes than requested even if the end of the stream has not been reached.</para>
        /// <para>Use <see cref="BinaryReader"/> for reading primitive data types.</para>
        /// <para>Please note that the behaviour described is not guaranteed as it is dependent upon the implementation of the wrapped streams. Refer to the documentation for the wrapped stream implementations for expected behaviour.</para>
        /// </remarks>
        public override int Read(byte[] buffer, int offset, int count)
        {
            // Check we haven't been disposed.
            if (_isDisposed)
            {
                // We have, so throw an exception.
                throw Exceptions.StreamDisposed();
            }

            // A variable to keep track of how many bytes we've read in total.
            var totalBytesRead = 0;

            // A flag to indicate whether we've managed to read all of the requested data from within the currently active stream,
            // or whether we need to grab the next stream and continue reading.
            var finished = false;

            // Go and grab the required stream.
            var stream = GetStream();

            // We'll keep looping to read data in case we need to read from more than one stream. If stream is null then we've reached
            // the end, so we'll just return what we have read.
            while (!finished && count > 0 && stream != null)
            {
                // We need to calculate the specified position within the active stream, so we offset the current position by the start
                // of the active stream within the single stream.
                var fileStreamPosition = (int)(Position - stream.Start);

                // Set a value of how many bytes we plan to read this iteration. If the requested bytes exceed this stream then this
                // value will be adjusted accordingly later on.
                var fileStreamCount = count;

                // Set the offset of where we need to write data into the target buffer. We keep this in it's own variable in case we
                // need to make another iteration.
                var bufferOffset = offset;

                // Check whether the requested bytes will exceed the end of the active stream.
                if (Position + count > stream.End)
                {
                    // The requested bytes cross the boundary of the active stream into the next stream, so we need to adjust some
                    // variables ready for the next iteration.

                    // First we adjust the number of bytes to read to the remaining length of this stream.
                    fileStreamCount = (int)(stream.Length - fileStreamPosition);

                    // Then we update the offset into the target buffer to account for the data we read from this stream. We'll update
                    // the "master" variable ready for the next iteration.
                    offset += fileStreamCount;

                    // As above, we update the bytes to read to account for the data we read from this stream. We'll update the "master"
                    // variable ready for the next iteration.
                    count = count - fileStreamCount;
                }
                else
                {
                    // We'll complete our read within this stream, so we can set our flag to true as we don't need another iteration of
                    // this loop.
                    finished = true;
                }

                // Set the correct position within our active stream.
                stream.Stream.Seek(fileStreamPosition, SeekOrigin.Begin);

                // Copy the required bytes from the stream into the buffer.
                var bytesRead = stream.Stream.Read(buffer, bufferOffset, fileStreamCount);

                // Increment our position according to how many bytes were read.
                Position += bytesRead;

                // Add the number of bytes read onto our total bytes read.
                totalBytesRead += bytesRead;

                // Check whether we need to do another loop.
                if (!finished)
                {
                    // We need to do another loop, which means we need data from beyond the end of the active stream and from the following
                    // stream. Our position has been updated so we can now get the next stream (which will also update the active stream)
                    // ready for the next iteration.
                    stream = GetStream();
                }
            }

            // Return how many bytes we actually read.
            return totalBytesRead;
        }
        /// <summary>
        /// Sets the position within the stream.
        /// </summary>
        /// <param name="offset">A byte offset relative to the <paramref name="origin"/> parameter.</param>
        /// <param name="origin">A value of type <see cref="SeekOrigin"/> indicating the reference point used to obtain the new position.</param>
        /// <returns>The new position within the stream.</returns>
        /// <exception cref="ArgumentException">There is an invalid <see cref="T:System.IO.SeekOrigin"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Attempted to set the position to a negative value.</exception>
        /// <exception cref="EndOfStreamException">Attempted seeking past the end of the stream.</exception>
        /// <exception cref="ObjectDisposedException">Methods were called after the stream was closed.</exception>
        /// <remarks>
        /// <para>Use the <see cref="CanSeek"/> property to determine whether the stream supports seeking.</para>
        /// <para>If <c>offset</c> is negative, the new position is required to precede the position specified by <c>origin</c> by the number of bytes specified by <c>offset</c>.If <c>offset</c> is zero(0), the new position is required to be the position specified by <c>origin</c>.If <c>offset</c> is positive, the new position is required to follow the position specified by <c>origin</c> by the number of bytes specified by <c>offset</c>.</para>
        /// </remarks>
        public override long Seek(long offset, SeekOrigin origin)
        {
            // Check we haven't been diposed.
            if (_isDisposed)
            {
                // We have, so throw an exception.
                throw Exceptions.StreamDisposed();
            }

            // Depending on the SeekOrigin type, we seek to the specified offset relative to the start, end, or current position of the
            // stream.
            switch (origin)
            {
                case SeekOrigin.Begin:
                    Position = offset;
                    break;
                case SeekOrigin.Current:
                    Position += offset;
                    break;
                case SeekOrigin.End:
                    Position = Length - offset;
                    break;
                default:
                    throw Exceptions.InvalidSeekOrigin();
            }

            // Now we check whether the specified position has exceeded the bounds of the stream and, if so, adjust the position to be in
            // bounds.
            if (Position > Length)
            {
                // Position has gone beyond the end of the stream, so set it to the end of the stream.
                Position = Length;
            }
            else if (Position < 0)
            {
                // Position has gone beyond the start of the stream, so set it to the start of the stream.
                Position = 0;
            }

            // Return the new position.
            return Position;
        }
        /// <summary>
        /// Sets the length of the wrapped stream
        /// </summary>
        /// <param name="value">The desired length of the stream in bytes.</param>
        /// <exception cref="NotSupportedException">The stream does not support both writing and seeking.</exception>
        /// <remarks>MultiStreams are read-only, so this method will always throw a <see cref="NotSupportedException"/>.</remarks>
        public override void SetLength(long value)
        {
            // If a stream does not support writing, the SetLength method should throw a NotSupportedException.
            //
            // https://learn.microsoft.com/en-us/dotnet/api/system.io.stream.setlength?view=net-9.0#exceptions
            // "NotSupportedException
            // The stream does not support both writing and seeking, such as if the stream is constructed from a pipe or console output."

            throw Exceptions.MultiStreamDoesNotSupportWriting();
        }
        /// <summary>
        /// Writes a block of bytes to the current stream using data read from a buffer.
        /// </summary>
        /// <param name="value">The value at which to set the length.</param>
        /// <exception cref="NotSupportedException">The current stream does not support writing.</exception>
        /// <remarks>MultiStreams are read-only, so this method will always throw a <see cref="NotSupportedException"/>.</remarks>
        /// 

        /// <summary>
        /// Writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.
        /// </summary>
        /// <param name="buffer">An array of bytes. This method copies <paramref name="count"/> bytes from <paramref name="buffer"/> to the current stream.</param>
        /// <param name="offset">The zero-based byte offset in <paramref name="buffer"/> at which to begin copying bytes to the current stream.</param>
        /// <param name="count">The number of bytes to be written to the current stream.</param>
        /// <exception cref="NotSupportedException">The stream does not support writing.</exception>
        /// <remarks>MultiStreams are read-only, so this method will always throw a <see cref="NotSupportedException"/>.</remarks>
        public override void Write(byte[] buffer, int offset, int count)
        {
            // If a stream does not support writing, the Write method should throw a NotSupportedException.
            //
            // https://learn.microsoft.com/en-us/dotnet/api/system.io.stream.write?view=net-9.0#system-io-stream-write(system-byte()-system-int32-system-int32)
            // "NotSupportedException
            // The stream does not support writing."

            throw Exceptions.MultiStreamDoesNotSupportWriting();
        }

        #endregion

        #endregion
    }
}