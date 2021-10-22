using D2RIpOverlayNet.DxFontLoader;
using GameOverlay.Drawing;
using GameOverlay.Windows;
using System;
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

        public OverlayWindow(int x, int y, int width, int height, bool bgVisible)
        {
            IpAddresses = new List<IPAddress>();
            LoadSampleDefaults(bgVisible);

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

        private void LoadSampleDefaults(bool bgVisible)
        {
            BackgroundColor = bgVisible ? System.Drawing.Color.FromArgb(0xE0, 0x33, 0x36, 0x3F) : System.Drawing.Color.FromArgb(0x00, 0x33, 0x36, 0x3F);
            // HeadlineFontColor = System.Drawing.Color.FromArgb(0xFF, 0xA5, 0x00);
            HeadlineFontColor = System.Drawing.Color.FromArgb(0xA5, 0x92, 0x63);
            RegularFontColor = System.Drawing.Color.FromArgb(0xFF, 0xFF, 0xFF);
            WrongIpColor = System.Drawing.Color.FromArgb(0xFF, 0x00, 0x00);
            RightIpColor = System.Drawing.Color.FromArgb(0x00, 0xFF, 0x00);

            var fam = new FontFamily("Segoe UI");
            var sz = 15D;
            var style = FontStyles.Normal;
            var strc = FontStretches.Normal;
            var weight = FontWeights.Normal;
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

            InitializeEmbeddedFonts(gfx);
        }

        private void InitializeEmbeddedFonts(Graphics gfx)
        {
            var factoryDWrite = gfx.GetFontFactory();
            var currentResourceFontLoader = new ResourceFontLoader(factoryDWrite);
            var currentFontCollection = new SharpDX.DirectWrite.FontCollection(factoryDWrite, currentResourceFontLoader, currentResourceFontLoader.Key);

            var hfWeight = FontConverter.GetFontWeight(HeadlineFont);
            var hfStyle = FontConverter.GetFontStyle(HeadlineFont);
            var hfStretch = FontConverter.GetFontStretch(HeadlineFont);
            var hfSize = (float)HeadlineFont.Size;

            var rfWeight = FontConverter.GetFontWeight(RegularFont);
            var rfStyle = FontConverter.GetFontStyle(RegularFont);
            var rfStretch = FontConverter.GetFontStretch(RegularFont);
            var rfSize = (float)RegularFont.Size;

            var hfTextFormat = new SharpDX.DirectWrite.TextFormat(factoryDWrite, "Exocet", currentFontCollection, hfWeight, hfStyle, hfStretch, hfSize);
            var rfTextFormat = new SharpDX.DirectWrite.TextFormat(factoryDWrite, "Exocet", currentFontCollection, rfWeight, rfStyle, rfStretch, rfSize);

            m_HeadlineFont = new Font(hfTextFormat);
            m_RegularFont = new Font(rfTextFormat);
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
            var yOffset = 3;
            var xOffset = 3;
            
            var gfx = e.Graphics;
            gfx.ClearScene(m_BackgroundColor);

            gfx.DrawText(m_HeadlineFont, m_HeadlineFontColor, xOffset + 0, yOffset + 0, "INGAME TIME:");
            gfx.DrawText(m_HeadlineFont, m_HeadlineFontColor, xOffset + 130, yOffset + 0, $"{InGameTime}");

            gfx.DrawText(m_HeadlineFont, m_HeadlineFontColor, xOffset + 0, yOffset + 18, "TARGET IP:");
            gfx.DrawText(m_HeadlineFont, m_HeadlineFontColor, xOffset + 0, yOffset + 36, "CURRENT IP:");
            gfx.DrawText(m_RegularFont, m_RegularFontColor, xOffset + 130, yOffset + 18, $"{TargetIpAddress}");


            for (int i = 0; i < IpAddresses.Count; i++)
            {
                var ipColor = IpAddresses[i].Equals(TargetIpAddress) ? m_RightIpColor : m_WrongIpColor;
                gfx.DrawText(m_RegularFont, ipColor, xOffset + 130, yOffset + (i + 2) * 18, $"{IpAddresses[i]}");
            }
        }

        public void Run()
        {
            m_Window.Create();
            m_Window.Join();
        }

        public void Hide() => m_Window.Hide();
        public void Show() => m_Window.Show();

        public void Rearrange(int x, int y, int width, int height, bool bgVisible)
        {
            m_Window.Move(x, y);
            m_Window.Resize(width, height);
            m_BackgroundColor.Color = bgVisible ? new GameOverlay.Drawing.Color(0x33, 0x36, 0x3F, 0xE0) : new GameOverlay.Drawing.Color(0x33, 0x36, 0x3F, 0x00);
        }
    }
}