using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using NetstatWrapper;

namespace Diablo2IpFinder
{
    public class IpFinder : INotifyPropertyChanged
    {
        // This is handled by the package PropertychangedFody
        // See https://github.com/Fody/PropertyChanged for details
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly string m_DiabloExecutable = "D2R";
        private bool m_InGame = false;
        private int m_InGameTime = 0;
        private readonly System.Timers.Timer m_InGameTimer;

        public IPAddress TargetIp { get; set; }
        public ObservableHashSet<IPAddress> ObservedIpAddresses { get; set; }
        public ObservableHashSet<IPAddress> IgnoredIpAddresses { get; set; }
        public ObservableHashSet<IPAddress> CurrentIpAddresses { get; set; }

        public int OverlayX { get; set; }
        public int OverlayY { get; set; }
        public int OverlayWidth { get; set; }
        public int OverlayHeight { get; set; }
        public bool OverlayBackgroundVisible { get; set; }
        public string InGameTime
        {
            get
            {
                var t = TimeSpan.FromMilliseconds(m_InGameTime);
                if (t.Days > 0) return $"{t.Days:D2} {t.Hours:D2}:{t.Minutes:D2}:{t.Seconds:D2}";
                else if (t.Hours > 0) return $"{t.Hours:D2}:{t.Minutes:D2}:{t.Seconds:D2}";
                else return $"{t.Minutes:D2}:{t.Seconds:D2}";
            }
        }

        public OverlayWindow D2Overlay { get; set; }

        public IpFinder(int x, int y, int width, int height, bool bgVisible)
        {
            ObservedIpAddresses = new ObservableHashSet<IPAddress>();
            IgnoredIpAddresses = new ObservableHashSet<IPAddress>();
            CurrentIpAddresses = new ObservableHashSet<IPAddress>();

            OverlayX = x;
            OverlayY = y;
            OverlayWidth = width;
            OverlayHeight = height;
            OverlayBackgroundVisible = bgVisible;

            D2Overlay = new OverlayWindow(OverlayX, OverlayY, OverlayWidth, OverlayHeight, OverlayBackgroundVisible);
            Task.Run(() => D2Overlay.Run());

            m_InGameTimer = new System.Timers.Timer(1000);
            m_InGameTimer.Elapsed += (o, e) =>
            {
                m_InGameTime += 1000;
                D2Overlay.InGameTime = InGameTime;
            };
            m_InGameTimer.AutoReset = true;
            m_InGameTimer.Enabled = true;
        }

        public void MainLoop()
        {
            while (true)
            {
                UpdateIpsNetstat();
                SynchronizeCollections();
                UpdateTimer();
                UpdateOverlay();

                Thread.Sleep(200);
            }
        }

        private void UpdateIpsNetstat()
        {
            var procId = Process.GetProcessesByName(m_DiabloExecutable).First().Id;

            var observedIPs = Netstat.GetExtendedTcpTable()
                                     .Where(con => !Helpers.IsFilteredIp(con.RemoteAddress) && 
                                            con.ProcessId == procId)
                                     .Select(con => con.RemoteAddress);

            CurrentIpAddresses.Clear();

            foreach (var ipAddr in observedIPs)
            {
                if (!IgnoredIpAddresses.Contains(ipAddr))
                {
                    ObservedIpAddresses.Add(ipAddr);
                    CurrentIpAddresses.Add(ipAddr);
                }
            }
        }

        private void SynchronizeCollections()
        {
            foreach (var ipAddr in ObservedIpAddresses)
            {
                if (IgnoredIpAddresses.Contains(ipAddr))
                {
                    ObservedIpAddresses.Remove(ipAddr);
                }
            }

            foreach (var ipAddr in CurrentIpAddresses)
            {
                if (IgnoredIpAddresses.Contains(ipAddr))
                {
                    CurrentIpAddresses.Remove(ipAddr);
                }
            }
            return;
        }

        private void UpdateOverlay()
        {
            D2Overlay.TargetIpAddress = TargetIp;

            D2Overlay.IpAddresses.Clear();
            D2Overlay.IpAddresses.AddRange(CurrentIpAddresses);
            return;
        }

        private void UpdateTimer()
        {
            if (m_InGame && CurrentIpAddresses.Count == 0)
            {
                m_InGame = false;
            }
            else if (!m_InGame && CurrentIpAddresses.Count > 0)
            {
                m_InGame = true;
                m_InGameTime = 0;

                if (!m_InGameTimer.Enabled) m_InGameTimer.Start();
            }
        }

        public void HideOverlay() => D2Overlay.Hide();
        public void ShowOverlay() => D2Overlay.Show();

        public void RearrangeOverlay() => D2Overlay.Rearrange(OverlayX, OverlayY, OverlayWidth, OverlayHeight, OverlayBackgroundVisible);
    }
}