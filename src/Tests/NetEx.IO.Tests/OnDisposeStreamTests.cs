using FluentAssertions;
using System.IO;
using System.IO.Compression;
using Xunit;

namespace NetEx.IO.Tests
{
    public class OnDisposeStreamTests
    {
        #region Constants

        private const string CompressedFileContents = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.";
        private const string CompressedFileName = "TestFile.txt";
        private const string ZipFilePath = @"TestData\OnDisposeStream\TestFile.zip";

        #endregion

        #region Fields

        private ZipArchive? _danglingZipFile;

        #endregion

        #region Methods

        public Stream? OpenStreamButCloseZipFile(string filePath)
        {
            using (var zipFile = ZipFile.OpenRead(filePath))
            {
                var compressedFile = zipFile.GetEntry(CompressedFileName);
                return compressedFile?.Open();
            }
        }
        public Stream? OpenStreamButKeepZipFileOpen(string filePath)
        {
            var zipFile = ZipFile.OpenRead(filePath);
            var compressedFile = zipFile.GetEntry(CompressedFileName);

            // Set _danglingZipFile to zipFile just so we can check it later to prove it has *not* been closed. This is only for testing
            // and would not be typically done in a real-world scenario.
            _danglingZipFile = zipFile;

            return compressedFile?.Open();
        }
        public Stream? OpenStreamUsingOnDisposeStream(string filePath)
        {
            var zipFile = ZipFile.OpenRead(filePath);
            var compressedFile = zipFile.GetEntry(CompressedFileName);
            if (compressedFile != null)
            {
                return new OnDisposeStream(compressedFile.Open(), b =>
                {
                    zipFile.Dispose();
                });
            }

            // Set _danglingZipFile to zipFile just so we can check it later to prove it *has* been closed. This is only for testing
            // and would not be typically done in a real-world scenario.
            _danglingZipFile = zipFile;

            return null;
        }

        #endregion

        #region Tests

        [Fact]
        public void OnDisposeStreamTest()
        {
            // We'll open a zip file and attempt to read the contents of the first file within it. However, the code to open the stream
            // is in a separate method and just returns the stream of the inner file back to us.
            //
            // In this scenario the "Open" method would typically either dispose of the `ZipFile` object once it completes, but this will
            // invalidate the stream returned, or it can leave the `ZipFile` object open, which will leave the stream valid but there is
            // then no way to dispose of the `ZipFile` object.
            //
            // Alternatively, the `ZipFile` object can be returned from the method, but this breaks the abstraction of just returning a
            // stream - for example, if the method was abstracting where the file came from, or if it could come from multple sources,
            // `Stream` would be the most appropriate return type. We'll test both of these scenarios using the `OpenStreamButCloseZipFile`
            // and `OpenStreamButKeepZipFileOpen` methods. Finally, we'll use the `OpenStreamUsingOnDisposeStream` method, which should
            // dipose of the `ZipFile` object when the stream is closed.

            // Check the test file exists.
            File.Exists(ZipFilePath).Should().BeTrue();

            // OpenStreamButCloseZipFile
            using (var compressedFileStream = OpenStreamButCloseZipFile(ZipFilePath))
            {
                // The stream should be valid, but the zip file is closed, so we can't read from it.
                compressedFileStream.Should().NotBeNull();
                compressedFileStream!.CanRead.Should().BeFalse();
            }

            // OpenStreamButKeepZipFileOpen
            _danglingZipFile = null;
            using (var compressedFileStream = OpenStreamButKeepZipFileOpen(ZipFilePath))
            {
                // The stream should be valid, and the zip file is open, so we can read from it.
                compressedFileStream.Should().NotBeNull();
                compressedFileStream!.CanRead.Should().BeTrue();

                // We'll read the contents of the file to confirm we have opened it.
                using (var reader = new StreamReader(compressedFileStream))
                {
                    var contents = reader.ReadToEnd();
                    contents.Should().Be(CompressedFileContents);
                }

                // The `ZipFile` object has been made an instance field purely for the purposes of testing, but this would not be the case
                // in a real-world scenario.

                // `StreamReader` closes the stream it uses, so our compressed file stream should now be disposed.
                compressedFileStream.Should().NotBeNull();
                compressedFileStream!.CanRead.Should().BeFalse();

                // However, the zip file is still open and usable.
                _danglingZipFile.Should().NotBeNull();
                _danglingZipFile!.Entries.Count.Should().Be(1);
            }

            // Even after we exit our `using` block for our stream, the zip file is stil open. And we have no way to dispose of the `ZipFile`
            // object, so it will remain open.
            _danglingZipFile.Should().NotBeNull();
            _danglingZipFile!.Entries.Count.Should().Be(1);

            // For the purposes of testing we'll dispose of the zip file manually, but again this is only for testing, and in a real-world
            // scenario we wouldn't have access to the zip file object.
            _danglingZipFile?.Dispose();

            // OpenStreamUsingOnDisposeStream
            _danglingZipFile = null;
            using (var compressedFileStream = OpenStreamUsingOnDisposeStream(ZipFilePath))
            {
                // The stream should be valid, and the zip file is open, so we can read from it.
                compressedFileStream.Should().NotBeNull();
                compressedFileStream!.CanRead.Should().BeTrue();

                // We'll read the contents of the file to confirm we have opened it.
                using (var reader = new StreamReader(compressedFileStream))
                {
                    var contents = reader.ReadToEnd();
                    contents.Should().Be(CompressedFileContents);
                }

                // As before, the `ZipFile` object has been made an instance field purely for the purposes of testing.

                // `StreamReader` closes the stream it uses, so our compressed file stream should now be disposed.
                compressedFileStream.Should().NotBeNull();
                compressedFileStream!.CanRead.Should().BeFalse();

                // And using `OnDisposeStream`, the `ZipFile` object should now also be closed.
                _danglingZipFile.Should().BeNull();
            }
        }

        #endregion
    }
}