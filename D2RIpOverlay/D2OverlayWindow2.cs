using GameOverlay.Drawing;
using GameOverlay.Windows;
using System.Collections.Generic;

namespace Diablo2IpFinder
{
    public class OverlayBrushes
    {
        private Graphics _gfx;
        public SolidBrush Black { get; private set; }
        public SolidBrush White { get; private set; }
        public SolidBrush Red { get; private set; }
        public SolidBrush Green { get; private set; }
        public SolidBrush Orange { get; private set; }
        public SolidBrush DarkGray { get; private set; }

        public OverlayBrushes(Graphics gfx)
        {
            _gfx = gfx;

            Black = _gfx.CreateSolidBrush(0, 0, 0);
            White = _gfx.CreateSolidBrush(255, 255, 255);
            Red = _gfx.CreateSolidBrush(255, 0, 0);
            Green = _gfx.CreateSolidBrush(0, 255, 0);
            Orange = _gfx.CreateSolidBrush(0xFF, 0xA5, 0x00);
            DarkGray = _gfx.CreateSolidBrush(0x33, 0x36, 0x3F, 0.93F);
        }
    }

    public class OverlayFonts
    {
        private Graphics _gfx;
        public Font Arial { get; set; }
        public Font Consolas { get; set; }

        public OverlayFonts(Graphics gfx)
        {
            _gfx = gfx;
            Arial = _gfx.CreateFont("Arial", 12);
            Consolas = _gfx.CreateFont("Consolas", 14);
        }
    }

    public class D2OverlayWindow2
    {
        private readonly GraphicsWindow _window;

        private OverlayBrushes _brushes;
        private OverlayFonts _fonts;

        public List<string> IpAddresses { get; set; }
        public string TargetIpAddress { get; set; }

        public D2OverlayWindow2(int x, int y, int width, int height)
        {
            IpAddresses = new List<string>();

            var gfx = new Graphics()
            {
                MeasureFPS = true,
                PerPrimitiveAntiAliasing = true,
                TextAntiAliasing = true
            };

            _window = new GraphicsWindow(x, y, width, height, gfx)
            {
                FPS = 60,
                IsTopmost = true,
                IsVisible = true
            };

            _window.DestroyGraphics += _window_DestroyGraphics;
            _window.DrawGraphics += _window_DrawGraphics;
            _window.SetupGraphics += _window_SetupGraphics;
        }

        private void _window_SetupGraphics(object sender, SetupGraphicsEventArgs e)
        {
            var gfx = e.Graphics;
            _brushes = new OverlayBrushes(gfx);
            _fonts = new OverlayFonts(gfx);

            if (e.RecreateResources)
            {
                _brushes.Black.Dispose();
                _brushes.Orange.Dispose();
                _brushes.DarkGray.Dispose();
                _brushes.Green.Dispose();
                _brushes.Red.Dispose();
                _brushes.White.Dispose();
            }

            if (e.RecreateResources) return;

            // _gridBounds = new Rectangle(20, 60, gfx.Width - 20, gfx.Height - 20);
            // _gridGeometry = gfx.CreateGeometry();
            // 
            // for (float y = _gridBounds.Top; y <= _gridBounds.Bottom; y += 20)
            // {
            //     var line = new Line(_gridBounds.Left, y, _gridBounds.Right, y);
            //     _gridGeometry.BeginFigure(line);
            //     _gridGeometry.EndFigure(false);
            // }
            // 
            // _gridGeometry.Close();
        }

        private void _window_DestroyGraphics(object sender, DestroyGraphicsEventArgs e)
        {
            _brushes.Black.Dispose();
            _brushes.Orange.Dispose();
            _brushes.DarkGray.Dispose();
            _brushes.Green.Dispose();
            _brushes.Red.Dispose();
            _brushes.White.Dispose();
            _fonts.Arial.Dispose();
            _fonts.Consolas.Dispose();
        }

        private void _window_DrawGraphics(object sender, DrawGraphicsEventArgs e)
        {
            var gfx = e.Graphics;

            gfx.ClearScene(_brushes.DarkGray);

            gfx.DrawText(_fonts.Consolas, _brushes.Orange, 0, 0, "Target IP:");
            gfx.DrawText(_fonts.Consolas, _brushes.Orange, 0, 18, "Current IP(s):");
            gfx.DrawText(_fonts.Consolas, _brushes.White, 112, 0, $"{TargetIpAddress}");

            for (int i = 0; i < IpAddresses.Count; i++)
            {
                gfx.DrawText(_fonts.Consolas, (IpAddresses[i] == TargetIpAddress) ? _brushes.Green : _brushes.Red, 112, (i + 1) * 18, IpAddresses[i]);
            }
        }

        public void Run()
        {
            _window.Create();
            _window.Join();
        }

        public void Hide() => _window.Hide();
        public void Show() => _window.Show();

        public void Rearrange(int x, int y, int width, int height)
        {
            _window.Move(x, y);
            _window.Resize(width, height);
        }
    }
}