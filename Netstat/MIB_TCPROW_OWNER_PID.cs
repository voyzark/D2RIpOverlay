using System;
using System.Runtime.InteropServices;

namespace NetstatD2
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct MIB_TCPROW_OWNER_PID
    {
        internal uint state;
        internal uint localAddr;
        internal byte localPort1;
        internal byte localPort2;
        internal byte localPort3;
        internal byte localPort4;
        internal uint remoteAddr;
        internal byte remotePort1;
        internal byte remotePort2;
        internal byte remotePort3;
        internal byte remotePort4;
        internal int owningPid;

        internal ushort LocalPort
        {
            get
            {
                return BitConverter.ToUInt16(
                    new byte[2] { localPort2, localPort1 }, 0);
            }
        }

        internal ushort RemotePort
        {
            get
            {
                return BitConverter.ToUInt16(
                    new byte[2] { remotePort2, remotePort1 }, 0);
            }
        }
    }
}
