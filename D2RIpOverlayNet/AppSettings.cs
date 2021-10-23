using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.Win32;

namespace Diablo2IpFinder
{
    public class AppSettings
    {
        #region Singleton Pattern (don't judge me, this seems reasonable here)
        private static AppSettings m_AppSettingsSingleton = null;
        public static AppSettings GetAppSettings
        {
            get
            {
                if (m_AppSettingsSingleton is null)
                {
                    m_AppSettingsSingleton = new AppSettings();
                }
                return m_AppSettingsSingleton;
            }
        }
        #endregion

        private readonly string m_RegistryPath = @"HKEY_CURRENT_USER\SOFTWARE\voyzark\d2ip";

        public ObservableHashSet<IPAddress> IgnoredIpAddresses { get; set; }
        public ObservableHashSet<IPAddress> ObservedIpAddresses { get; set; }
        public IPAddress TargetIpAddress { get; set; }

        public int OverlayX { get; set; }
        public int OverlayY { get; set; }
        public int OverlayWidth { get; set; }
        public int OverlayHeight { get; set; }
        public bool OverlayBackgroundVisible { get; set; }


        private AppSettings()
        {
            ReadSettings();
        }

        public void ReadSettings()
        {
            var ignoredIpAddresses = ((string[])ReadKeyOrDefault(m_RegistryPath, nameof(IgnoredIpAddresses), Array.Empty<string>()))
                                        .Select(addr => IPAddress.TryParse(addr, out IPAddress parsedAddr) ? parsedAddr : null)
                                        .Where(addr => addr != null);

            var observedIpAddresses = ((string[])ReadKeyOrDefault(m_RegistryPath, nameof(ObservedIpAddresses), Array.Empty<string>()))
                                        .Select(addr => IPAddress.TryParse(addr, out IPAddress parsedAddr) ? parsedAddr : null)
                                        .Where(addr => addr != null && !ignoredIpAddresses.Contains(addr));

            IgnoredIpAddresses = ObservableHashSet<IPAddress>.FromIEnumerable(ignoredIpAddresses);
            ObservedIpAddresses = ObservableHashSet<IPAddress>.FromIEnumerable(observedIpAddresses);

            TargetIpAddress = IPAddress.TryParse((string)ReadKeyOrDefault(m_RegistryPath, nameof(TargetIpAddress), String.Empty), out IPAddress outAddr) ? outAddr : null;

            OverlayX = (int)ReadKeyOrDefault(m_RegistryPath, nameof(OverlayX), 0);
            OverlayY = (int)ReadKeyOrDefault(m_RegistryPath, nameof(OverlayY), 0);
            OverlayWidth = (int)ReadKeyOrDefault(m_RegistryPath, nameof(OverlayWidth), 250);
            OverlayHeight = (int)ReadKeyOrDefault(m_RegistryPath, nameof(OverlayHeight), 58);
            OverlayBackgroundVisible = Boolean.Parse(ReadKeyOrDefault(m_RegistryPath, nameof(OverlayBackgroundVisible), false).ToString());
        }

        public void WriteSettings()
        {
            Registry.SetValue(m_RegistryPath, nameof(IgnoredIpAddresses), IgnoredIpAddresses.Select(addr => addr.ToString()).ToArray());
            Registry.SetValue(m_RegistryPath, nameof(ObservedIpAddresses), IgnoredIpAddresses.Select(addr => addr.ToString()).ToArray());
            Registry.SetValue(m_RegistryPath, nameof(TargetIpAddress), TargetIpAddress is null ? "" : TargetIpAddress.ToString());
            Registry.SetValue(m_RegistryPath, nameof(OverlayX), OverlayX);
            Registry.SetValue(m_RegistryPath, nameof(OverlayY), OverlayY);
            Registry.SetValue(m_RegistryPath, nameof(OverlayWidth), OverlayWidth);
            Registry.SetValue(m_RegistryPath, nameof(OverlayHeight), OverlayHeight);
            Registry.SetValue(m_RegistryPath, nameof(OverlayBackgroundVisible), OverlayBackgroundVisible);
        }

        private object ReadKeyOrDefault(string keyName, string valueName, object defaultValue)
        {
            var ret = Registry.GetValue(keyName, valueName, defaultValue);
            return (ret is null) ? defaultValue : ret;
        }
    }
}
