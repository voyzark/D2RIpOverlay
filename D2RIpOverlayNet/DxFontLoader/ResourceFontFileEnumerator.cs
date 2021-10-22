using SharpDX;
using SharpDX.DirectWrite;

namespace D2RIpOverlayNet.DxFontLoader
{
    /// <summary>
    /// Resource FontFileEnumerator.
    /// </summary>
    public class ResourceFontFileEnumerator : CallbackBase, FontFileEnumerator
    {
        private Factory m_Factory;
        private FontFileLoader m_Loader;
        private DataStream m_KeyStream;
        private FontFile m_CurrentFontFile;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceFontFileEnumerator"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="loader">The loader.</param>
        /// <param name="key">The key.</param>
        public ResourceFontFileEnumerator(Factory factory, FontFileLoader loader, DataPointer key)
        {
            m_Factory = factory;
            m_Loader = loader;
            m_KeyStream = new DataStream(key.Pointer, key.Size, true, false);
        }

        /// <summary>
        /// Advances to the next font file in the collection. When it is first created, the enumerator is positioned before the first element of the collection and the first call to MoveNext advances to the first file.
        /// </summary>
        /// <returns>
        /// the value TRUE if the enumerator advances to a file; otherwise, FALSE if the enumerator advances past the last file in the collection.
        /// </returns>
        /// <unmanaged>HRESULT IDWriteFontFileEnumerator::MoveNext([Out] BOOL* hasCurrentFile)</unmanaged>
        bool FontFileEnumerator.MoveNext()
        {
            bool moveNext = m_KeyStream.RemainingLength != 0;
            if (moveNext)
            {
                if (m_CurrentFontFile != null)
                    m_CurrentFontFile.Dispose();

                m_CurrentFontFile = new FontFile(m_Factory, m_KeyStream.PositionPointer, 4, m_Loader);
                m_KeyStream.Position += 4;
            }
            return moveNext;
        }

        /// <summary>
        /// Gets a reference to the current font file.
        /// </summary>
        /// <value></value>
        /// <returns>a reference to the newly created <see cref="SharpDX.DirectWrite.FontFile"/> object.</returns>
        /// <unmanaged>HRESULT IDWriteFontFileEnumerator::GetCurrentFontFile([Out] IDWriteFontFile** fontFile)</unmanaged>
        FontFile FontFileEnumerator.CurrentFontFile
        {
            get
            {
                ((IUnknown)m_CurrentFontFile).AddReference();
                return m_CurrentFontFile;
            }
        }
    }
}
