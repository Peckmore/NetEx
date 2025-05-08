using System;
using System.Collections.Generic;
using System.IO;

namespace NetEx.IO
{
    /// <summary>
    /// Creates a wrapper around a multiple <see cref="Stream"/> instances, and presents them as a single, read-only stream.
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
        public MultiStream(IEnumerable<Stream> streams)
        {
            // We need to create our list where we'll store a stream for each file.
            _fileStreams = new List<StreamInfo>();

            foreach (var stream in streams)
            {
                _fileStreams.Add(new StreamInfo(stream, _length));

                _length += stream.Length;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// The name of the <see cref="Stream"/> currently being accessed within the <see cref="MultiStream"/>, based on <see cref="Position"/>.
        /// </summary>
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

                return _activeStream?.Name;
            }
        }
        /// <inheritdoc/>
        public sealed override bool CanRead => !_isDisposed;
        /// <inheritdoc/>
        public sealed override bool CanSeek => !_isDisposed;
        /// <inheritdoc/>
        public sealed override bool CanTimeout => false;
        /// <inheritdoc/>
        public sealed override bool CanWrite => false;
        /// <inheritdoc/>
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

                return _length;
            }
        }
        /// <inheritdoc/>
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

                return _position;
            }
            set => _position = value;
        }
        /// <inheritdoc/>
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
        /// <inheritdoc/>
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
                // Close each of our file streams.
                foreach (var fileStream in _fileStreams)
                {
                    fileStream.Dispose();
                }
                _fileStreams.Clear();

                // Call the OnDispose() method in any derived classes.
                //OnDispose(disposing);

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

        /// <inheritdoc/>
        public override void Flush()
        {
            // https://learn.microsoft.com/en-us/dotnet/api/system.io.stream.flush?view=net-8.0#remarks
            // "In a class derived from Stream that doesn't support writing, Flush is typically implemented as an empty method to
            // ensure full compatibility with other Stream types since it's valid to flush a read-only stream."

            // Check we haven't been disposed.
            if (_isDisposed)
            {
                // We have, so throw an exception.
                throw Exceptions.StreamDisposed();
            }
        }
        /// <inheritdoc/>
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
        /// Sets the position within the current stream to the specified value.
        /// </summary>
        /// <param name="offset">The new position within the stream. This is relative to the <code>loc</code> parameter, and can be positive or negative.</param>
        /// <param name="loc">A value of type <see cref="SeekOrigin"/>, which acts as the seek reference point.</param>
        /// <returns></returns>
        public override long Seek(long offset, SeekOrigin loc)
        {
            // Check we haven't been diposed.
            if (_isDisposed)
            {
                // We have, so throw an exception.
                throw Exceptions.StreamDisposed();
            }

            // Depending on the SeekOrigin type, we seek to the specified offset relative to the start, end, or current position of the
            // stream.
            switch (loc)
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
        /// <inheritdoc/>
        public override void SetLength(long value)
        {
            // If a stream does not support writing, the SetLength method should throw a NotSupportedException.
            //
            // https://learn.microsoft.com/en-us/dotnet/api/system.io.stream.setlength?view=net-9.0#exceptions
            // "NotSupportedException
            // The stream does not support both writing and seeking, such as if the stream is constructed from a pipe or console output."

            throw Exceptions.MultiStreamDoesNotSupportWriting();
        }
        /// <inheritdoc/>
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