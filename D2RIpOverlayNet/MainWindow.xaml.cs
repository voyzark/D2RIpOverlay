using System;
using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Diablo2IpFinder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IpFinder m_Finder;
        private OverlaySettings m_OverlaySettings;
        private static readonly AppSettings m_AppSettings = AppSettings.GetAppSettings;

        public MainWindow()
        {
            Console.WriteLine("Starting");
            InitializeComponent();
            InititalizeData();

            // tbSetIpStatus.FontFamily = 

            DataContext = m_Finder;

            Task.Run(() => m_Finder.MainLoop());
        }

        private void InititalizeData()
        {
            btnToggleOverlay.Tag = true;

            m_Finder = new IpFinder(m_AppSettings.OverlayX, m_AppSettings.OverlayY, m_AppSettings.OverlayWidth, m_AppSettings.OverlayHeight, m_AppSettings.OverlayBackgroundVisible)
            {
                ObservedIpAddresses = m_AppSettings.ObservedIpAddresses,
                IgnoredIpAddresses = m_AppSettings.IgnoredIpAddresses,
                TargetIp = m_AppSettings.TargetIpAddress
            };
            tbSetIpStatus.Text = m_Finder.TargetIp is null ? "Target IP is not set" : $"Target IP Address set to {m_Finder.TargetIp}";
            m_OverlaySettings = new OverlaySettings();
            m_OverlaySettings.SettingsChanged += OnSettingsChanged;

        }

        private void OnSettingsChanged(object sender, EventArgs e)
        {
            m_Finder.OverlayX = m_AppSettings.OverlayX;
            m_Finder.OverlayY = m_AppSettings.OverlayY;
            m_Finder.OverlayWidth = m_AppSettings.OverlayWidth;
            m_Finder.OverlayHeight = m_AppSettings.OverlayHeight;
            m_Finder.OverlayBackgroundVisible = m_AppSettings.OverlayBackgroundVisible;

            m_Finder.RearrangeOverlay();
        }

        private void SetIp_Click(object sender, RoutedEventArgs e)
        {
            // this should help against race conditions (e.g. tbTargetIp.Text changes right after the check)
            // it also prettifies status output
            var targetIp = Helpers.NormalizeIp(tbTargetIp.Text);
            if (String.IsNullOrWhiteSpace(targetIp))
            {
                m_Finder.TargetIp = null;
                m_AppSettings.TargetIpAddress = null;
                tbSetIpStatus.Text = "Target IP is not set";
                tbTargetIp.Background = Brushes.White;
            }
            else if (Helpers.IsValidIp(targetIp))
            {
                m_Finder.TargetIp = IPAddress.Parse(targetIp);
                m_AppSettings.TargetIpAddress = m_Finder.TargetIp;
                tbSetIpStatus.Text = $"Target IP Address set to {targetIp}";
                tbTargetIp.Background = Brushes.White;
            }
        }

        private void AddIgnoredIp_Click(object sender, RoutedEventArgs e)
        {
            // this should help against race conditions (e.g. tbIgnoredIp.Text changes right after the check)
            var targetIp = tbIgnoredIp.Text;
            if (Helpers.IsValidIp(targetIp))
            {
                tbIgnoredIp.Background = Brushes.White;
                m_Finder.IgnoredIpAddresses.Add(IPAddress.Parse(targetIp));
            }
            else
            {
                tbIgnoredIp.Background = Brushes.LightPink;
            }
        }

        private void MoveIpRight_Click(object sender, RoutedEventArgs e)
        {
            var selectedIpAddress = (IPAddress)lvObservedIpAddresses.SelectedItem;
            m_Finder.ObservedIpAddresses.Remove(selectedIpAddress);
            m_Finder.IgnoredIpAddresses.Add(selectedIpAddress);
        }

        private void MoveIpLeft_Click(object sender, RoutedEventArgs e)
        {
            var selectedIpAddress = (IPAddress)lvIgnoredIpAddresses.SelectedItem;
            m_Finder.IgnoredIpAddresses.Remove(selectedIpAddress);
            m_Finder.ObservedIpAddresses.Add(selectedIpAddress);
        }

        private void ToggleOverlay_Click(object sender, RoutedEventArgs e)
        {
            // Overlay is Visible
            if ((bool)btnToggleOverlay.Tag)
            {
                m_Finder.HideOverlay();
                btnToggleOverlay.Tag = false;
                btnToggleOverlay.Content = "Show";
            }
            else // Overlay is Hidden
            {
                m_Finder.ShowOverlay();
                btnToggleOverlay.Tag = true;
                btnToggleOverlay.Content = "Hide";
            }
        }
        
        private void ShowSettings_Click(object sender, RoutedEventArgs e)
        {
            m_OverlaySettings?.Show();
        }

        private void TargetIp_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(tbTargetIp.Text) && Helpers.IsValidIp(tbTargetIp.Text))
            {
                tbTargetIp.Background = Brushes.LightGreen;
            }
            else
            {
                tbTargetIp.Background = Brushes.LightPink;
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            m_Finder.HideOverlay();
            m_AppSettings.WriteSettings();
            Application.Current.Shutdown();
        }
    }
}
