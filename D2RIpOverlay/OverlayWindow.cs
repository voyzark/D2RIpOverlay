using GameOverlay.Drawing;
using GameOverlay.Windows;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Media;
using WpfColorFontDialog;

namespace Diablo2IpFinder
{
    public class OverlayWindow
    {
        #region private Fields 
        private readonly GraphicsWindow m_Window;

        private SolidBrush m_BackgroundColor;
        private SolidBrush m_HeadlineFontColor;
        private SolidBrush m_RegularFontColor;
        private SolidBrush m_WrongIpColor;
        private SolidBrush m_RightIpColor;
        private Font m_HeadlineFont;
        private Font m_RegularFont;
        #endregion

        #region Public Properties
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string InGameTime { get; set; }
        public IPAddress TargetIpAddress { get; set; }
        public List<IPAddress> IpAddresses { get; set; }

        public System.Drawing.Color BackgroundColor { get; set; }
        public System.Drawing.Color HeadlineFontColor { get; set; }
        public System.Drawing.Color RegularFontColor { get; set; }
        public System.Drawing.Color WrongIpColor { get; set; }
        public System.Drawing.Color RightIpColor { get; set; }
        public FontInfo HeadlineFont { get; set; }
        public FontInfo RegularFont { get; set; }
        #endregion

        public OverlayWindow(int x, int y, int width, int height)
        {
            IpAddresses = new List<IPAddress>();
            LoadSampleDefaults();

            var gfx = new Graphics()
            {
                MeasureFPS = true,
                PerPrimitiveAntiAliasing = true,
                TextAntiAliasing = true
            };

            m_Window = new GraphicsWindow(x, y, width, height, gfx)
            {
                FPS = 60,
                IsTopmost = true,
                IsVisible = true
            };

            m_Window.SetupGraphics += m_Window_SetupGraphics;
            m_Window.DrawGraphics += m_Window_DrawGraphics;
            m_Window.DestroyGraphics += m_Window_DestroyGraphics;
        }

        private void LoadSampleDefaults()
        {
            BackgroundColor = System.Drawing.Color.FromArgb(230, 0x33, 0x36, 0x3F);
            HeadlineFontColor = System.Drawing.Color.FromArgb(0xFF, 0xA5, 0x00);
            RegularFontColor = System.Drawing.Color.FromArgb(0xFF, 0xFF, 0xFF);
            WrongIpColor = System.Drawing.Color.FromArgb(0xFF, 0x00, 0x00);
            RightIpColor = System.Drawing.Color.FromArgb(0x00, 0xFF, 0x00);

            var fam = new FontFamily("Segoe UI");
            var sz = 12D;
            var style = FontStyles.Normal;
            var strc = FontStretches.Normal;
            var weight = FontWeights.Bold;
            var c = Brushes.Black;

            HeadlineFont = new FontInfo(fam, sz, style, strc, weight, c);
            RegularFont = new FontInfo(fam, sz, style, strc, weight, c);
        }

        private void m_Window_SetupGraphics(object sender, SetupGraphicsEventArgs e)
        {
            var gfx = e.Graphics;

            if (e.RecreateResources)
            {
                m_BackgroundColor.Dispose();
                m_HeadlineFontColor.Dispose();
                m_RegularFontColor.Dispose();
                m_WrongIpColor.Dispose();
                m_RightIpColor.Dispose();
                m_HeadlineFont.Dispose();
                m_RegularFont.Dispose();
                return;
            }

            m_BackgroundColor = gfx.CreateSolidBrush(BackgroundColor.R, BackgroundColor.G, BackgroundColor.B, BackgroundColor.A);
            m_HeadlineFontColor = gfx.CreateSolidBrush(HeadlineFontColor.R, HeadlineFontColor.G, HeadlineFontColor.B, HeadlineFontColor.A);
            m_RegularFontColor = gfx.CreateSolidBrush(RegularFontColor.R, RegularFontColor.G, RegularFontColor.B, RegularFontColor.A);
            m_WrongIpColor = gfx.CreateSolidBrush(WrongIpColor.R, WrongIpColor.G, WrongIpColor.B, WrongIpColor.A);
            m_RightIpColor = gfx.CreateSolidBrush(RightIpColor.R, RightIpColor.G, RightIpColor.B, RightIpColor.A);

            m_HeadlineFont = gfx.CreateFont(HeadlineFont.Family.ToString(), (float)HeadlineFont.Size, HeadlineFont.Weight > FontWeights.Regular,
                                            HeadlineFont.Style == FontStyles.Italic);
            m_RegularFont = gfx.CreateFont(RegularFont.Family.ToString(), (float)RegularFont.Size, RegularFont.Weight > FontWeights.Regular,
                                            RegularFont.Style == FontStyles.Italic);
        }

        private void m_Window_DestroyGraphics(object sender, DestroyGraphicsEventArgs e)
        {
            m_BackgroundColor.Dispose();
            m_HeadlineFontColor.Dispose();
            m_RegularFontColor.Dispose();
            m_WrongIpColor.Dispose();
            m_RightIpColor.Dispose();
            m_HeadlineFont.Dispose();
            m_RegularFont.Dispose();
        }

        private void m_Window_DrawGraphics(object sender, DrawGraphicsEventArgs e)
        {
            var gfx = e.Graphics;
            gfx.ClearScene(m_BackgroundColor);

            gfx.DrawText(m_HeadlineFont, m_HeadlineFontColor, 0, 0, "Ingame Time:");
            gfx.DrawText(m_HeadlineFont, m_HeadlineFontColor, 112, 0, $"{InGameTime}");

            gfx.DrawText(m_HeadlineFont, m_HeadlineFontColor, 0, 18, "Target IP:");
            gfx.DrawText(m_HeadlineFont, m_HeadlineFontColor, 0, 36, "Current IP(s):");
            gfx.DrawText(m_RegularFont, m_RegularFontColor, 112, 18, $"{TargetIpAddress}");


            for (int i = 0; i < IpAddresses.Count; i++)
            {
                var ipColor = IpAddresses[i].Equals(TargetIpAddress) ? m_RightIpColor : m_WrongIpColor;
                gfx.DrawText(m_RegularFont, ipColor, 112, (i + 2) * 18, $"{IpAddresses[i]}");
            }
        }

        public void Run()
        {
            m_Window.Create();
            m_Window.Join();
        }

        public void Hide() => m_Window.Hide();
        public void Show() => m_Window.Show();

        public void Rearrange(int x, int y, int width, int height)
        {
            m_Window.Move(x, y);
            m_Window.Resize(width, height);
        }
    }
}
