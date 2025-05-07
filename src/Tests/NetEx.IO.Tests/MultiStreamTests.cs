using FluentAssertions;
using NetEx.IO;
using System;
using System.Collections.Generic;
using System.IO;

namespace NetEx.Windows.Forms.Tests
{
    public class StreamTests
    {
        #region Constants

        private const int RandomSeekIterations = 10000;

        #endregion

        #region Methods

        #region Private

        private void CheckDisposedStream(MultiStream stream)
        {
            // Check that the following properties return the correct values.
            stream.CanRead.Should().BeFalse();
            stream.CanSeek.Should().BeFalse();
            stream.CanTimeout.Should().BeFalse();
            stream.CanWrite.Should().BeFalse();

            // Check that the following properties and methods throw an ObjectDisposedException.
            var testString = () => _ = stream.ActiveStreamName;
            testString.Should().Throw<ObjectDisposedException>();

            var testLong = () => _ = stream.Length;
            testLong.Should().Throw<ObjectDisposedException>();

            testLong = () => _ = stream.Position;
            testLong.Should().Throw<ObjectDisposedException>();

            var testVoid = () => stream.Flush();
            testVoid.Should().Throw<ObjectDisposedException>();

            var testInt = () => stream.Read(new byte[1], 0, 1);
            testInt.Should().Throw<ObjectDisposedException>();

            var testByte = () => stream.ReadByte();
            testByte.Should().Throw<ObjectDisposedException>();

            testLong = () => stream.Seek(0, SeekOrigin.Begin);
            testLong.Should().Throw<ObjectDisposedException>();
        }
        private bool CheckResults(byte[] data, int expectedSize, long position)
        {
            // This is a helper method to iterate through a byte array and check that the value at each byte is of the expected value.
            // The position of the first byte within the file it is taken from is supplied, meaning we can calculate the expected value
            // at any position within the array, and check it against the actual value.

            for (int x = 0; x < expectedSize; x++)
            {
                if (data[x] != GetSequentialValue(position + x))
                {
                    return false;
                }
            }

            return true;
        }
        private byte GetSequentialValue(long position)
        {
            // Because we know that all of our data files consist of the values 0-255, repeating for the length of the file, we can
            // determine the correct value at any position within a file by taking the modulus of the position wth 256.

            return (byte)(position % 256);
        }

        #endregion

        #region Public Static

        /// <summary>
        /// Gets a <see cref="MultiStream"/> instance for our test data.
        /// </summary>
        public static IEnumerable<object[]> GetMultiStream()
        {
            var streams = new List<Stream>();
            for (var x = 1; x < 9; x++)
            {
                streams.Add(File.Open(@$"TestData\{x}.file", FileMode.Open));
            }

            yield return new[] { new MultiStream(streams) };
        }

        #endregion

        #endregion

        #region Tests

        /// <summary>
        /// Verify that <see cref="MultiStream.CanRead"/> returns <see langword="true"/>.
        /// </summary>
        [Theory]
        [MemberData(nameof(GetMultiStream))]
        public void CanRead(MultiStream  stream)
        {
            stream.Should().NotBeNull();

            stream.CanRead.Should().BeTrue();
        }
        /// <summary>
        /// Verify that <see cref="MultiStream.CanSeek"/> returns <see langword="true"/>.
        /// </summary>
        [Theory]
        [MemberData(nameof(GetMultiStream))]
        public void CanSeek(MultiStream stream)
        {
            stream.Should().NotBeNull();

            stream.CanSeek.Should().BeTrue();
        }
        /// <summary>
        /// Verify that <see cref="MultiStream.CanTimeout"/> returns <see langword="false"/>.
        /// </summary>
        [Theory]
        [MemberData(nameof(GetMultiStream))]
        public void CanTimeout(MultiStream stream)
        {
            stream.Should().NotBeNull();

            stream.CanTimeout.Should().BeFalse();
        }
        /// <summary>
        /// Verify that <see cref="MultiStream.CanWrite"/> returns <see langword="false"/>.
        /// </summary>
        [Theory]
        [MemberData(nameof(GetMultiStream))]
        public void CanWrite(MultiStream stream)
        {
            stream.Should().NotBeNull();

            stream.CanWrite.Should().BeFalse();
        }
        [Theory]
        [MemberData(nameof(GetMultiStream))]
        public void Close(MultiStream stream)
        {
            stream.Should().NotBeNull();

            // Close the stream.
            stream.Close();

            // Check it has closed/disposed correctly.
            CheckDisposedStream(stream);
        }
        [Theory]
        [MemberData(nameof(GetMultiStream))]
        public void Dispose(MultiStream stream)
        {
            stream.Should().NotBeNull();

            // Dispose the stream.
            stream.Dispose();

            // Check it has closed/disposed correctly.
            CheckDisposedStream(stream);
        }
        /// <summary>
        /// Read the entire stream in 63 byte chunks using <see cref="MultiStream.Read"/> and verify that each byte is correct.
        /// </summary>
        /// <remarks>63 byte chunks were chosen as a value not divisible by 8 to ensure that inner stream boundaries would occur
        /// mid-read and are handled correctly.</remarks>
        [Theory]
        [MemberData(nameof(GetMultiStream))]
        public void Read(MultiStream stream)
        {
            stream.Should().NotBeNull();

            // Create a variable to track our expected position within the stream.
            var position = 0;

            // Loop through the length of the stream.
            while (position < stream.Length)
            {
                // Create our 63 byte array to read data into.
                var data = new byte[63];

                // Read 63 bytes from the stream (using it's own internal position).
                var bytesRead = stream.Read(data, 0, data.Length);

                // Check that the data in the array matches what we expect.
                CheckResults(data, bytesRead, position).Should().BeTrue();

                // Increment our position tracker.
                position += data.Length;
            }
        }
        /// <summary>
        /// Read the entire stream one byte at a time using <see cref="MultiStream.ReadByte"/> and verify that each byte is correct.
        /// </summary>
        [Theory]
        [MemberData(nameof(GetMultiStream))]
        public void ReadByte(MultiStream stream)
        {
            stream.Should().NotBeNull();

            // Create a variable to track what we expect each read byte to be. Because we are reading one byte at a time we can simply
            // increase this variable by one after each read. And as it is a byte, once we get to 255 it will wrap around back to 0.
            byte compareValue = 0;

            // A variable to track whether any comparisons have failed.
            var fail = false;

            // Loop through the length of the stream.
            for (var position = 0; position < stream.Length; position++)
            {
                // Read a byte from the stream.
                var b = (byte)stream.ReadByte();

                // Check that the value matches what we expect.
                if (b != compareValue)
                {
                    // If we're here then the value didn't match, so set our fail flag to true.
                    fail = true;

                    // There's no point checking any further of the stream as the test has failed, so abort the loop.
                    break;
                }

                // Increment our expected read value, ready for the next byte.
                compareValue++;
            }

            // Indicate whether the test passed or failed.
            fail.Should().BeFalse();
        }
        /// <summary>
        /// Verify that <see cref="MultiStream"/> throws an exception if <see cref="MultiStream.ReadTimeout"/> is accessed.
        /// </summary>
        [Theory]
        [MemberData(nameof(GetMultiStream))]
        public void ReadTimeout(MultiStream stream)
        {
            stream.Should().NotBeNull();

            var test = () => stream.ReadTimeout;
            test.Should().Throw<InvalidOperationException>();
        }
        /// <summary>
        /// Perform random seeks across a <see cref="MultiStream"/> and read chunks of data in increasing size using <see cref="MultiStream.ReadByte"/>
        /// and verify that each byte is correct.
        /// </summary>
        [Theory]
        [MemberData(nameof(GetMultiStream))]
        public void Seek(MultiStream stream)
        {
            stream.Should().NotBeNull();

            // We need an RNG to generate random seek amounts.
            var rnd = new Random();

            // We'll test all 3 SeekOrigin types, and we'll perform several thousand seeks per stream to make sure there are no issues with
            // any positions, boundaries, etc. We'll also increment how many bytes we read each time (from 1 to our iteration count) to
            // stress all different types of reads, boundaries, etc.

            // SeekOrigin.Begin
            for (var count = 1; count <= RandomSeekIterations; count++)
            {
                // Generate a new offset for how far into the stream we'll jump (from 0). We subtract count to ensure we'll always have
                // enough bytes to read.
                var offset = rnd.NextInt64(0, stream.Length - count);

                // Store the expected stream position.
                var expectedPosition = offset;

                // Set the position by performing a seek.
                stream.Seek(offset, SeekOrigin.Begin);

                // Verify that the position is correct.
                stream.Position.Should().Be(expectedPosition);

                // Create our byte array to read data into.
                var data = new byte[count];

                // Read the bytes from the stream (using it's own internal position now that we've set it).
                var bytesRead = stream.Read(data, 0, data.Length);

                // Check that the data in the array matches what we expect.
                CheckResults(data, bytesRead, expectedPosition).Should().BeTrue();
            }

            // SeekOrigin.Current
            
            // To seek based on the current position we'll switch direction (forwards or backwards) depending on how close we are to the
            // start or end of the stream. This boolean will track our direction.
            bool negative = false;

            for (var count = 1; count <= RandomSeekIterations; count++)
            {
                if (stream.Position > (0.9 * stream.Length))
                {
                    // If we're near the end of the stream, switch to moving backwards.
                    negative = true;
                }
                else if (stream.Position < (0.1 * stream.Length))
                {
                    // Else if we're near the start of the stream, switch to moving forwards.
                    negative = false;
                }

                // Variables to store offset and expected position.
                long expectedPosition;
                long offset;
                if (negative)
                {
                    // We're moving backwards, so our offset has to be negative, and somewhere between where we are now (stream.Position)
                    // and the start of the stream. We buffer by the length of count to ensure there is enough data to read.
                    offset = rnd.NextInt64(-stream.Position, -count);

                    // Store the expected stream position.
                    expectedPosition = stream.Position + offset;
                }
                else
                {
                    // We're moving forwards, so our offset has to be positive, and somewhere between where we are now and the end of
                    // the stream (stream.Length - stream.Position). We buffer by the length of count to ensure there is enough data to read.
                    offset = rnd.NextInt64(0, stream.Length - stream.Position - count);

                    // Store the expected stream position.
                    expectedPosition = stream.Position + offset;
                }

                // Set the position by performing a seek.
                stream.Seek(offset, SeekOrigin.Current);

                // Verify that the position is correct.
                stream.Position.Should().Be(expectedPosition);

                // Create our byte array to read data into.
                var data = new byte[count];

                // Read the bytes from the stream (using it's own internal position now that we've set it).
                var bytesRead = stream.Read(data, 0, data.Length);

                // Check that the data in the array matches what we expect.
                CheckResults(data, bytesRead, expectedPosition).Should().BeTrue();
            }

            // SeekOrigin.End
            for (var count = 1; count <= RandomSeekIterations; count++)
            {
                // Generate a new offset for how far into the stream we'll jump (from the end). We add count to ensure we'll always have
                // enough bytes to read.
                var offset = rnd.NextInt64(count, stream.Length);

                // Store the expected stream position.
                var expectedPosition = stream.Length - offset;

                // Set the position by performing a seek.
                stream.Seek(offset, SeekOrigin.End);

                // Verify that the position is correct.
                stream.Position.Should().Be(expectedPosition);

                // Create our byte array to read data into.
                var data = new byte[count];

                // Read the bytes from the stream (using it's own internal position now that we've set it).
                var bytesRead = stream.Read(data, 0, data.Length);

                // Check that the data in the array matches what we expect.
                CheckResults(data, bytesRead, expectedPosition).Should().BeTrue();
            }
        }
        /// <summary>
        /// Verify that a <see cref="MultiStream"/> throws an exception if <see cref="MultiStream.SetLength"/> is accessed.
        /// </summary>
        [Theory]
        [MemberData(nameof(GetMultiStream))]
        public void SetLength(MultiStream stream)
        {
            stream.Should().NotBeNull();

            var test = () => stream.SetLength(10);
            test.Should().Throw<NotSupportedException>();
        }
        /// <summary>
        /// Verify that a <see cref="MultiStream"/> throws an exception if <see cref="MultiStream.Write"/> is accessed.
        /// </summary>
        [Theory]
        [MemberData(nameof(GetMultiStream))]
        public void Write(MultiStream stream)
        {
            stream.Should().NotBeNull();

            var test = () => stream.Write(new byte[10], 0, 10);
            test.Should().Throw<NotSupportedException>();
        }
        /// <summary>
        /// Verify that a <see cref="MultiStream"/> throws an exception if <see cref="MultiStream.WriteByte"/> is accessed.
        /// </summary>
        [Theory]
        [MemberData(nameof(GetMultiStream))]
        public void WriteByte(MultiStream stream)
        {
            stream.Should().NotBeNull();

            var test = () => stream.WriteByte(0);
            test.Should().Throw<NotSupportedException>();
        }
        /// <summary>
        /// Verify that a <see cref="MultiStream"/> throws an exception if <see cref="MultiStream.WriteTimeout"/> is accessed.
        /// </summary>
        [Theory]
        [MemberData(nameof(GetMultiStream))]
        public void WriteTimeout(MultiStream stream)
        {
            stream.Should().NotBeNull();

            var test = () => stream.WriteTimeout;
            test.Should().Throw<InvalidOperationException>();
        }

        #endregion
    }
}