using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace NetstatWrapper
{
    public static class Netstat
    {        
        private static IEnumerable<TcpConnectionInfo> GetTable(string type)
        {
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = Path.Combine(Environment.ExpandEnvironmentVariables("SYSTEM32"),
                                            "netstat.exe"),
                    Arguments = String.IsNullOrWhiteSpace(type) ? "-ano" : $"-anop {type}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                }
            };

            proc.Start();
            while (!proc.StandardOutput.EndOfStream)
            {
                string line = proc.StandardOutput.ReadLine();
                if (TcpConnectionInfo.TryParse(line, out TcpConnectionInfo connectionInfo))
                    yield return connectionInfo;
            }
        }

        public static IEnumerable<TcpConnectionInfo> GetExtendedIpTable() => GetTable(String.Empty);
        public static IEnumerable<TcpConnectionInfo> GetExtendedTcpTable() => GetTable("TCP");
        public static IEnumerable<TcpConnectionInfo> GetExtendedUdpTable() => GetTable("UPD");
    }
}
