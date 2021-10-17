using System.Runtime.InteropServices;

namespace NetstatD2
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct MIB_TCPTABLE_OWNER_PID
    {
        internal uint dwNumEntries;
        MIB_TCPROW_OWNER_PID table;
    }
}
