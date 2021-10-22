using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfColorFontDialog;
using System.Windows;

namespace D2RIpOverlayNet.DxFontLoader
{
    public static class FontConverter
    {
        public static SharpDX.DirectWrite.FontWeight GetFontWeight(FontWeight fontWeight)
        {
            if (fontWeight.Equals(FontWeights.Thin))
                return SharpDX.DirectWrite.FontWeight.Thin;
            if (fontWeight.Equals(FontWeights.ExtraLight))
                return SharpDX.DirectWrite.FontWeight.ExtraLight;
            if (fontWeight.Equals(FontWeights.UltraLight))
                return SharpDX.DirectWrite.FontWeight.UltraLight;
            if (fontWeight.Equals(FontWeights.Light))
                return SharpDX.DirectWrite.FontWeight.Light;
            if (fontWeight.Equals(FontWeights.Normal))
                return SharpDX.DirectWrite.FontWeight.Normal;
            if (fontWeight.Equals(FontWeights.Medium))
                return SharpDX.DirectWrite.FontWeight.Medium;
            if (fontWeight.Equals(FontWeights.DemiBold))
                return SharpDX.DirectWrite.FontWeight.DemiBold;
            if (fontWeight.Equals(FontWeights.SemiBold))
                return SharpDX.DirectWrite.FontWeight.SemiBold;
            if (fontWeight.Equals(FontWeights.Bold))
                return SharpDX.DirectWrite.FontWeight.Bold;
            if (fontWeight.Equals(FontWeights.ExtraBold))
                return SharpDX.DirectWrite.FontWeight.ExtraBold;
            if (fontWeight.Equals(FontWeights.UltraBold))
                return SharpDX.DirectWrite.FontWeight.UltraBold;
            if (fontWeight.Equals(FontWeights.Black))
                return SharpDX.DirectWrite.FontWeight.Black;
            if (fontWeight.Equals(FontWeights.Heavy))
                return SharpDX.DirectWrite.FontWeight.Heavy;
            if (fontWeight.Equals(FontWeights.ExtraBlack))
                return SharpDX.DirectWrite.FontWeight.ExtraBlack;
            if (fontWeight.Equals(FontWeights.UltraBlack))
                return SharpDX.DirectWrite.FontWeight.UltraBlack;

            return SharpDX.DirectWrite.FontWeight.Regular;
        }
        public static SharpDX.DirectWrite.FontWeight GetFontWeight(FontInfo fontInfo) => GetFontWeight(fontInfo.Weight);

        public static SharpDX.DirectWrite.FontStyle GetFontStyle(FontStyle fontStyle)
        {
            if (fontStyle.Equals(FontStyles.Italic))
                return SharpDX.DirectWrite.FontStyle.Italic;
            if (fontStyle.Equals(FontStyles.Oblique))
                return SharpDX.DirectWrite.FontStyle.Oblique;

            return SharpDX.DirectWrite.FontStyle.Normal;
        }
        public static SharpDX.DirectWrite.FontStyle GetFontStyle(FontInfo fontInfo) => GetFontStyle(fontInfo.Style);

        public static SharpDX.DirectWrite.FontStretch GetFontStretch(FontStretch fontStretch)
        {
            if (fontStretch.Equals(FontStretches.UltraCondensed))
                return SharpDX.DirectWrite.FontStretch.UltraCondensed;
            if (fontStretch.Equals(FontStretches.ExtraCondensed))
                return SharpDX.DirectWrite.FontStretch.ExtraCondensed;
            if (fontStretch.Equals(FontStretches.Condensed))
                return SharpDX.DirectWrite.FontStretch.Condensed;
            if (fontStretch.Equals(FontStretches.SemiCondensed))
                return SharpDX.DirectWrite.FontStretch.SemiCondensed;
            if (fontStretch.Equals(FontStretches.Medium))
                return SharpDX.DirectWrite.FontStretch.Medium;
            if (fontStretch.Equals(FontStretches.SemiExpanded))
                return SharpDX.DirectWrite.FontStretch.SemiExpanded;
            if (fontStretch.Equals(FontStretches.Expanded))
                return SharpDX.DirectWrite.FontStretch.Expanded;
            if (fontStretch.Equals(FontStretches.ExtraExpanded))
                return SharpDX.DirectWrite.FontStretch.ExtraExpanded;
            if (fontStretch.Equals(FontStretches.UltraExpanded))
                return SharpDX.DirectWrite.FontStretch.UltraExpanded;

            return SharpDX.DirectWrite.FontStretch.Normal;
        }
        public static SharpDX.DirectWrite.FontStretch GetFontStretch(FontInfo fontInfo) => GetFontStretch(fontInfo.Stretch);
    }
}
