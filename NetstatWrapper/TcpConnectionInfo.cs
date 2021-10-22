using System;
using System.Net;
using System.Text.RegularExpressions;

namespace NetstatWrapper
{
    public class TcpConnectionInfo
    {
        private static readonly Regex re = new Regex(@"^\s*(?<Type>[^\s]*?)\s+(?<LocalIp>[^\s]*):(?<LocalPort>[^\s]*?)\s+(?<RemoteIp>[^\s]*):(?<RemotePort>[^\s]*?)\s+(?<State>[^\s]*)\s+(?<ProcId>[^\s]*)\s*$");
        private static Regex ValidIpSchema = new Regex(@"^(?:[0-9]{1,3}\.){3}([0-9]{1,3})$");

        public IPAddress LocalAddress { get; set; }
        public IPAddress RemoteAddress { get; set; }
        public ushort LocalPort { get; set; }
        public ushort RemotePort { get; set; }
        public int ProcessId { get; set; }

        public TcpConnectionInfo()
        {

        }

        internal static bool TryParse(string inputLine, out TcpConnectionInfo tcpConnectionInfo)
        {
            tcpConnectionInfo = new TcpConnectionInfo();
            var match = re.Match(inputLine);

            if (!match.Success)
                return false;

            string localAddr = match.Groups["LocalIp"].Value;
            string remoteAddr = match.Groups["RemoteIp"].Value;
            string localPort = match.Groups["LocalPort"].Value;
            string remotePort = match.Groups["RemotePort"].Value;
            string procId = match.Groups["ProcId"].Value;

            if (IsValidIp(localAddr)) tcpConnectionInfo.LocalAddress = IPAddress.Parse(localAddr);
            else return false;
            if (IsValidIp(remoteAddr)) tcpConnectionInfo.RemoteAddress = IPAddress.Parse(remoteAddr);
            else return false;
            if (UInt16.TryParse(localPort, out ushort lp)) tcpConnectionInfo.LocalPort = lp;
            else return false;
            if (UInt16.TryParse(remotePort, out ushort rp)) tcpConnectionInfo.RemotePort = rp;
            else return false;
            if (Int32.TryParse(procId, out int pid)) tcpConnectionInfo.ProcessId = pid;
            else return false;

            return true;
        }

        private static bool IsValidIp(string ipAddress)
        {
            var match = ValidIpSchema.Match(ipAddress);
            return match.Success;
        }
    }
}
