using SharpDX;
using SharpDX.DirectWrite;
using System.Collections.Generic;

namespace D2RIpOverlayNet.DxFontLoader
{

    /// <summary>
    /// ResourceFont main loader. This classes implements FontCollectionLoader and FontFileLoader.
    /// It reads all fonts embedded as resource in the current assembly and expose them.
    /// </summary>
    public partial class ResourceFontLoader : CallbackBase, FontCollectionLoader, FontFileLoader
    {
        private readonly List<ResourceFontFileStream> m_FontStreams = new List<ResourceFontFileStream>();
        private readonly List<ResourceFontFileEnumerator> m_Enumerators = new List<ResourceFontFileEnumerator>();
        private DataStream m_KeyStream;
        private readonly Factory m_Factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceFontLoader"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        public ResourceFontLoader(Factory factory)
        {
            m_Factory = factory;
            foreach (var name in typeof(ResourceFontLoader).Assembly.GetManifestResourceNames())
            {
                if (name.EndsWith(".ttf"))
                {
                    var fontBytes = Utilities.ReadStream(typeof(ResourceFontLoader).Assembly.GetManifestResourceStream(name));
                    var stream = new DataStream(fontBytes.Length, true, true);
                    stream.Write(fontBytes, 0, fontBytes.Length);
                    stream.Position = 0;
                    m_FontStreams.Add(new ResourceFontFileStream(stream));
                }
            }

            // Build a Key storage that stores the index of the font
            m_KeyStream = new DataStream(sizeof(int) * m_FontStreams.Count, true, true);
            for (int i = 0; i < m_FontStreams.Count; i++)
                m_KeyStream.Write((int)i);
            m_KeyStream.Position = 0;

            // Register the 
            m_Factory.RegisterFontFileLoader(this);
            m_Factory.RegisterFontCollectionLoader(this);
        }

        private void RebuildFontStreams()
        {
            m_FontStreams.Clear();

            foreach (var name in typeof(ResourceFontLoader).Assembly.GetManifestResourceNames())
            {
                if (name.EndsWith(".ttf"))
                {
                    var fontBytes = Utilities.ReadStream(typeof(ResourceFontLoader).Assembly.GetManifestResourceStream(name));
                    var stream = new DataStream(fontBytes.Length, true, true);
                    stream.Write(fontBytes, 0, fontBytes.Length);
                    stream.Position = 0;
                    m_FontStreams.Add(new ResourceFontFileStream(stream));
                }
            }

            // Build a Key storage that stores the index of the font
            m_KeyStream = new DataStream(sizeof(int) * m_FontStreams.Count, true, true);
            for (int i = 0; i < m_FontStreams.Count; i++)
                m_KeyStream.Write((int)i);
            m_KeyStream.Position = 0;
        }

        /// <summary>
        /// Gets the key used to identify the FontCollection as well as storing index for fonts.
        /// </summary>
        /// <value>The key.</value>
        public DataStream Key
        {
            get
            {
                return m_KeyStream;
            }
        }

        /// <summary>
        /// Creates a font file enumerator object that encapsulates a collection of font files. The font system calls back to this interface to create a font collection.
        /// </summary>
        /// <param name="factory">Pointer to the <see cref="SharpDX.DirectWrite.Factory"/> object that was used to create the current font collection.</param>
        /// <param name="collectionKey">A font collection key that uniquely identifies the collection of font files within the scope of the font collection loader being used. The buffer allocated for this key must be at least  the size, in bytes, specified by collectionKeySize.</param>
        /// <returns>
        /// a reference to the newly created font file enumerator.
        /// </returns>
        /// <unmanaged>HRESULT IDWriteFontCollectionLoader::CreateEnumeratorFromKey([None] IDWriteFactory* factory,[In, Buffer] const void* collectionKey,[None] int collectionKeySize,[Out] IDWriteFontFileEnumerator** fontFileEnumerator)</unmanaged>
        FontFileEnumerator FontCollectionLoader.CreateEnumeratorFromKey(Factory factory, DataPointer collectionKey)
        {
            var enumerator = new ResourceFontFileEnumerator(factory, this, collectionKey);
            m_Enumerators.Add(enumerator);

            return enumerator;
        }

        /// <summary>
        /// Creates a font file stream object that encapsulates an open file resource.
        /// </summary>
        /// <param name="fontFileReferenceKey">A reference to a font file reference key that uniquely identifies the font file resource within the scope of the font loader being used. The buffer allocated for this key must at least be the size, in bytes, specified by  fontFileReferenceKeySize.</param>
        /// <returns>
        /// a reference to the newly created <see cref="SharpDX.DirectWrite.FontFileStream"/> object.
        /// </returns>
        /// <remarks>
        /// The resource is closed when the last reference to fontFileStream is released.
        /// </remarks>
        /// <unmanaged>HRESULT IDWriteFontFileLoader::CreateStreamFromKey([In, Buffer] const void* fontFileReferenceKey,[None] int fontFileReferenceKeySize,[Out] IDWriteFontFileStream** fontFileStream)</unmanaged>
        FontFileStream FontFileLoader.CreateStreamFromKey(DataPointer fontFileReferenceKey)
        {
            RebuildFontStreams();
            var index = Utilities.Read<int>(fontFileReferenceKey.Pointer);
            var fontStream = m_FontStreams[index];
            return fontStream;
        }
    }
}