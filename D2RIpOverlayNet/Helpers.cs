using System;
using System.Text.RegularExpressions;
using System.Net;
using System.Collections.Generic;

namespace Diablo2IpFinder
{
    static class Helpers
    {
        private static readonly Regex validIpSchema = new Regex(@"^(?:[0-9]{1,3}\.){3}([0-9]{1,3})$");
        private static readonly IPAddress Localhost = IPAddress.Parse("127.0.0.1");

        private static readonly Regex FilteredIpRanges = new Regex(
            @"(?:^127\.0\.0\.1)" + "|" +
            @"(?:^24\.105\.29\.76)" + "|" +
            @"(?:^34\.117\.122\.6)" + "|" +
            @"(?:^37\.244\.28\.[0-9]{1,3})" + "|" +
            @"(?:^37\.244\.54\.[0-9]{1,3})" + "|" +
            @"(?:^117\.52\.35\.[0-9]{1,3})" + "|" +
            @"(?:^137\.221\.105\.[0-9]{1,3})" + "|" +
            @"(?:^137\.221\.106\.[0-9]{1,3})"
        );

        public static bool IsValidIp(string ipAddress)
        {
            var match = validIpSchema.Match(ipAddress);
            return match.Success && Int32.Parse(match.Groups[1].Value) > 0;
        }

        public static string NormalizeIp(string ipAddress)
        {
            if (!IsValidIp(ipAddress))
                return String.Empty;

            return IPAddress.Parse(ipAddress).ToString();
        }

        public static bool IsLocalhost(IPAddress ipAddr) => IPAddress.Equals(ipAddr, Localhost);

        public static bool IsFilteredIp(IPAddress ipAddr) => FilteredIpRanges.IsMatch(ipAddr.ToString());
    }
}
