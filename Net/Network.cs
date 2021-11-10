using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ProtoSim.DotNetUtilities.Net {
    public static class Network {
        public static bool IsConnected => NetworkInterface.GetIsNetworkAvailable();
        public static IPAddress? IPv4Address => NetworkInterface.GetAllNetworkInterfaces().Where(i => i.OperationalStatus.Equals(OperationalStatus.Up)).FirstOrDefault()?.GetIPProperties().UnicastAddresses.Where(i => i.Address.AddressFamily.Equals(AddressFamily.InterNetwork)).FirstOrDefault()?.Address;

        public static List<IPAddress>? GetNetworkIPAddresses(IPAddress? baseAddress = null) {
            if (!IsConnected) {
                Debug.WriteLine("not connected");
                return null;
            }

            var ipAddress = (baseAddress ?? IPv4Address)?.ToString().Split('.') ?? new string[4];
            var replies = new List<PingReply>();
            var tasks = new List<Task>();
            Parallel.For(0, 255, i => tasks.Add(Task.Factory.StartNew(() => replies.Add(new Ping().Send($"{ipAddress[0]}.{ipAddress[1]}.{ipAddress[2]}.{i}", 10)), TaskCreationOptions.LongRunning)));
            Task.WaitAll(tasks.ToArray());
            return replies.Where(i => i.Status.Equals(IPStatus.Success)).Select(i => i.Address).Distinct().ToList();
        }
    }
}