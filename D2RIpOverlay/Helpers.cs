using System;
using System.Text.RegularExpressions;
using System.Net;

namespace Diablo2IpFinder
{
    static class Helpers
    {
        private static Regex validIpSchema = new Regex(@"^(?:[0-9]{1,3}\.){3}([0-9]{1,3})$");

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
    }
}
