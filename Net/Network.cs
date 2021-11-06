using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace ProtoSim.DotNetUtilities.Net {
    public static class Network {
        public static bool IsConnected => NetworkInterface.GetIsNetworkAvailable();
        public static IPAddress? IPv4Address => NetworkInterface.GetAllNetworkInterfaces().Where(i => i.NetworkInterfaceType.Equals(NetworkInterfaceType.Ethernet) && i.OperationalStatus.Equals(OperationalStatus.Up)).FirstOrDefault()?.GetIPProperties().UnicastAddresses.Where(i => i.Address.AddressFamily.Equals(AddressFamily.InterNetwork)).FirstOrDefault()?.Address;
    }
}