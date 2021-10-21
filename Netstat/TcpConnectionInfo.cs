using System;
using System.Diagnostics;
using System.Net;

namespace NetstatD2
{
    public class TcpConnectionInfo
    {
        public IPAddress LocalAddress { get; private set; }
        public IPAddress RemoteAddress { get; private set; }
        public ushort LocalPort { get; private set; }
        public ushort RemotePort { get; private set; }
        public int OwnerPid { get; private set; }
        public string OwnerProcessName { get; private set; }

        internal TcpConnectionInfo(MIB_TCPROW_OWNER_PID mibTcpRowOwnerPid)
        {
            LocalPort = mibTcpRowOwnerPid.LocalPort;
            RemotePort = mibTcpRowOwnerPid.RemotePort;

            LocalAddress = ParseIpAddr(mibTcpRowOwnerPid.localAddr);
            RemoteAddress = ParseIpAddr(mibTcpRowOwnerPid.remoteAddr);

            OwnerPid = mibTcpRowOwnerPid.owningPid;
            OwnerProcessName = Process.GetProcessById(OwnerPid).ProcessName;
        }

        public static bool TryParse(string localAddress, string localPort, string remoteAddress, string remotePort, string ownerPid, out TcpConnectionInfo tcpConnectionInfo)
        {
            try
            {
                tcpConnectionInfo = new TcpConnectionInfo(localAddress, localPort, remoteAddress, remotePort, ownerPid);
                return true;
            } catch (Exception)
            {
                tcpConnectionInfo = null;
                return false;
            }
        }

        public TcpConnectionInfo(string localAddress, string localPort, string remoteAddress, string remotePort, string ownerPid) 
        {
            LocalPort = ushort.Parse(localPort);
            RemotePort = ushort.Parse(remotePort);

            LocalAddress = IPAddress.Parse(localAddress);
            RemoteAddress = IPAddress.Parse(remoteAddress);

            OwnerPid = int.Parse(ownerPid);
            OwnerProcessName = Process.GetProcessById(OwnerPid).ProcessName;
        }

        private IPAddress ParseIpAddr(uint ipaddr)
        {
            var ipAsBytes = new byte[4]
            {
                (byte)(ipaddr & 0xFF),
                (byte)(ipaddr >> 8 & 0xFF),
                (byte)(ipaddr >> 16 & 0xFF),
                (byte)(ipaddr >> 24 & 0xFF),
            };

            return new IPAddress(ipAsBytes);
        }

        public override string ToString()
        {
            return $"{LocalAddress}:{LocalPort} -> {RemoteAddress}:{RemotePort} [{OwnerProcessName}]";
        }
    }
}