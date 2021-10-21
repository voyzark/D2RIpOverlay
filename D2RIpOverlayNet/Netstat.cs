using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation.Runspaces;
using System.Management.Automation;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Net;
using NetstatD2;

namespace Diablo2IpFinder
{
    public static class Netstat
    {
        public static Regex re = new Regex(@"^\s*(?<Type>[^\s]*?)\s+(?<LocalIp>[^\s]*):(?<LocalPort>[^\s]*?)\s+(?<RemoteIp>[^\s]*):(?<RemotePort>[^\s]*?)\s+(?<State>[^\s]*)\s+(?<ProcId>[^\s]*)\s*$");

        public static IEnumerable<TcpConnectionInfo> GetTcpConnections(int procIdFilter)
        {
            Runspace runSpace = RunspaceFactory.CreateRunspace();
            runSpace.Open();
            Pipeline pipeline = runSpace.CreatePipeline();
            Command cmd = new Command("netstat");
            cmd.Parameters.Add("-anob");
            pipeline.Commands.Add(cmd);



            var outputs = pipeline.Invoke();
            foreach (var group in outputs.Where(x => re.IsMatch((string)x.BaseObject))
                                         .Select(x => re.Match((string)x.BaseObject)?.Groups)
                                         .Where(x => int.Parse(x["ProcId"].Value) == procIdFilter &&
                                                               IPAddress.TryParse(x["RemoteIp"].Value, out IPAddress addr) &&
                                                               !addr.Equals(IPAddress.Parse("127.0.0.1")) &&
                                                               int.Parse(x["RemotePort"].Value) == 443))
            {
                
                
                foreach (var gp in group)
                {
                    Trace.Write(gp + ", ");
                }
                Trace.WriteLine("");


                if (TcpConnectionInfo.TryParse(group["LocalIp"].Value, group["LocalPort"].Value, group["RemoteIp"].Value,
                                               group["RemotePort"].Value, group["ProcId"].Value, out TcpConnectionInfo connectionInfo))
                {
                    yield return connectionInfo;
                }
            }
        }
    }
}