using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace System.Windows.Forms
{
    /// <summary>
    /// Represents a resource that contains an Audio-Video Interleaved (AVI) clip to run in a dialog box.
    /// </summary>
    [CLSCompliant(false)]
    public class AnimationResource
    {
        #region Construction

        /// <summary>
        /// Initializes a new <see cref="AnimationResource"/> using a specified file and resource index.
        /// </summary>
        /// <param name="fileName">The filename of the resource containing the Audio-Video Interleaved (AVI) clip.</param>
        /// <param name="resourceIndex">The index of the Audio-Video Interleaved (AVI) clip within the resources contained in the file.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="fileName" /> is null or empty.
        /// </exception>
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        public AnimationResource(string fileName, ushort resourceIndex)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));

            FileName = fileName;
            ResourceIndex = resourceIndex;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The filename of the resource containing the Audio-Video Interleaved (AVI) clip.
        /// </summary>
        public string FileName { get; }
        /// <summary>
        /// The index of the Audio-Video Interleaved (AVI) clip within the resources contained in the file.
        /// </summary>
        public ushort ResourceIndex { get; }

        #endregion

        #region Methods

        #region Operators

        /// <summary>Determines whether two specified <see cref="AnimationResource"/> objects are equal.</summary>
        /// <returns>true if <paramref name="left"/> equals <paramref name="right"/>; otherwise, false.</returns>
        /// <param name="left">The first <see cref="Version" /> object.</param>
        /// <param name="right">The second <see cref="Version" /> object.</param>
        public static bool operator ==(AnimationResource left, AnimationResource right) => left?.FileName == right?.FileName && left?.ResourceIndex == right?.ResourceIndex;
        /// <summary>Determines whether two specified <see cref="AnimationResource"/> objects are not equal.</summary>
        /// <returns>true if <paramref name="left" /> does not equal <paramref name="right" />; otherwise, false.</returns>
        /// <param name="left">The first <see cref="AnimationResource"/> object.</param>
        /// <param name="right">The second <see cref="AnimationResource"/> object.</param>
        public static bool operator !=(AnimationResource left, AnimationResource right) => !(left == right);

        #endregion

        #region Public

        /// <summary>
        /// Indicates whether the specified object is an <see cref="AnimationResource"/> and has the same <see cref="FileName"/> and <see cref="ResourceIndex"/> property values as this <see cref="AnimationResource"/>.
        /// </summary>
        /// <param name="obj">The object to test.</param>
        /// <returns>true if the <paramref name="obj"/> parameter is an <see cref="AnimationResource"/> and has the same <see cref="FileName"/> and <see cref="ResourceIndex"/> property values as this <see cref="AnimationResource"/>; otherwise, false.</returns>
        public override bool Equals(object obj) => obj as AnimationResource == this;
        /// <summary>
        /// Gets the hash code for this <see cref="AnimationResource"/>.
        /// </summary>
        /// <returns>The hash code for this <see cref="AnimationResource"/>.</returns>
        public override int GetHashCode() => FileName.GetHashCode() ^ ResourceIndex.GetHashCode();
        /// <summary>
        /// Returns a string representation of this <see cref="AnimationResource"/>.
        /// </summary>
        /// <returns>A string that represents this <see cref="AnimationResource"/>.</returns>
        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append(GetType().Name);
            builder.Append(" [");
            builder.Append("File=\"");
            builder.Append(FileName);
            builder.Append("\", ResourceIndex=");
            builder.Append(ResourceIndex);
            builder.Append("]");

            return builder.ToString();
        }

        #endregion

        #region Public Static

        /// <summary>
        /// Gets an <see cref="AnimationResource"/> that represents the set multiple file attributes animation.
        /// </summary>
        /// <returns>An <see cref="AnimationResource"/> representing the set multiple file attributes animation.</returns>
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public static AnimationResource ApplySettings => new AnimationResource("shell32.dll", 165);
        /// <summary>
        /// Gets an <see cref="AnimationResource"/> that represents the folder-to-folder file copy animation.
        /// </summary>
        /// <returns>An <see cref="AnimationResource"/> representing the folder-to-folder file copy animation.</returns>
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public static AnimationResource CopyFile => new AnimationResource("shell32.dll", 161);
        /// <summary>
        /// Gets an <see cref="AnimationResource"/> that represents the folder-to-nothing file delete animation.
        /// </summary>
        /// <returns>An <see cref="AnimationResource"/> representing the folder-to-nothing file delete animation.</returns>
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public static AnimationResource DeleteFile => new AnimationResource("shell32.dll", 164);
        /// <summary>
        /// Gets an <see cref="AnimationResource"/> that represents the globe-to-folder file download animation.
        /// </summary>
        /// <returns>An <see cref="AnimationResource"/> representing the globe-to-folder file download animation.</returns>
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public static AnimationResource DownloadFile => new AnimationResource("shell32.dll", 170);
        /// <summary>
        /// Gets an <see cref="AnimationResource"/> that represents the Recycle Bin file delete animation.
        /// </summary>
        /// <returns>An <see cref="AnimationResource"/> representing the Recycle Bin file delete animation.</returns>
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public static AnimationResource EmptyRecycleBin => new AnimationResource("shell32.dll", 163);
        /// <summary>
        /// Gets an <see cref="AnimationResource"/> that represents the folder-to-folder file move animation.
        /// </summary>
        /// <returns>An <see cref="AnimationResource"/> representing the folder-to-folder file move animation.</returns>
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public static AnimationResource MoveFile => new AnimationResource("shell32.dll", 160);
        /// <summary>
        /// Gets an <see cref="AnimationResource"/> that represents the folder-to-recycle bin file delete animation.
        /// </summary>
        /// <returns>An <see cref="AnimationResource"/> representing the folder-to-recycle bin file delete animation.</returns>
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public static AnimationResource RecycleFile => new AnimationResource("shell32.dll", 162);
        /// <summary>
        /// Gets an <see cref="AnimationResource"/> that represents the torch over folder animation.
        /// </summary>
        /// <returns>An <see cref="AnimationResource"/> representing the torch over folder animation.</returns>
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public static AnimationResource Search => new AnimationResource("shell32.dll", 150);
        /// <summary>
        /// Gets an <see cref="AnimationResource"/> that represents the magnifying glass over globe animation.
        /// </summary>
        /// <returns>An <see cref="AnimationResource"/> representing the magnifying glass over globe animation.</returns>
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public static AnimationResource SearchGlobe => new AnimationResource("shell32.dll", 166);

        #endregion

        #endregion
    }
}