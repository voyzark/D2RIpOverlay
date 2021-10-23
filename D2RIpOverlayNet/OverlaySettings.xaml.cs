using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace Diablo2IpFinder
{
    /// <summary>
    /// Interaktionslogik für OverlaySettings.xaml
    /// </summary>
    public partial class OverlaySettings : Window
    {
        public EventHandler<EventArgs> SettingsChanged;
        
        private readonly AppSettings m_AppSettings = AppSettings.GetAppSettings;
        private int m_X;
        private int m_Y;
        private int m_Width;
        private int m_Height;
        private bool m_OverlayBackgroundTransparent;

        public OverlaySettings()
        {
            InitializeComponent();

            m_X = m_AppSettings.OverlayX;
            m_Y = m_AppSettings.OverlayY;
            m_Width = m_AppSettings.OverlayWidth;
            m_Height = m_AppSettings.OverlayHeight;
            m_OverlayBackgroundTransparent = m_AppSettings.OverlayBackgroundVisible;

            tbX.Text = $"{m_X}";
            tbY.Text = $"{m_Y}";
            tbW.Text = $"{m_Width}";
            tbH.Text = $"{m_Height}";
            cbTv.IsChecked = m_OverlayBackgroundTransparent;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            // base.OnClosing(e);
            Hide();
            e.Cancel = true;
        }

        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            bool parsingError = false;

            if (!Int32.TryParse(tbX.Text, out int x))
            {
                tbX.Background = Brushes.LightPink;
                parsingError = true;
            }

            if (!Int32.TryParse(tbY.Text, out int y))
            {
                tbY.Background = Brushes.LightPink;
                parsingError = true;
            }

            if (!Int32.TryParse(tbW.Text, out int w))
            {
                tbW.Background = Brushes.LightPink;
                parsingError = true;
            }

            if (!Int32.TryParse(tbH.Text, out int h))
            {
                tbH.Background = Brushes.LightPink;
                parsingError = true;
            }

            if (parsingError)
                return;

            m_X = x;
            m_Y = y;
            m_Width = w;
            m_Height = h;
            m_OverlayBackgroundTransparent = cbTv.IsChecked is null || (bool)cbTv.IsChecked;

            m_AppSettings.OverlayX = m_X;
            m_AppSettings.OverlayY = m_Y;
            m_AppSettings.OverlayWidth = m_Width;
            m_AppSettings.OverlayHeight = m_Height;
            m_AppSettings.OverlayBackgroundVisible = m_OverlayBackgroundTransparent;

            tbX.Background = Brushes.White;
            tbY.Background = Brushes.White;
            tbW.Background = Brushes.White;
            tbH.Background = Brushes.White;

            RaiseSettingsChanged();
        }

        private void RaiseSettingsChanged() => SettingsChanged?.Invoke(this, new EventArgs());

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            tbX.Text = $"{m_X}";
            tbY.Text = $"{m_Y}";
            tbW.Text = $"{m_Width}";
            tbH.Text = $"{m_Height}";

            tbX.Background = Brushes.White;
            tbY.Background = Brushes.White;
            tbW.Background = Brushes.White;
            tbH.Background = Brushes.White;

            Hide();
        }        
    }
}