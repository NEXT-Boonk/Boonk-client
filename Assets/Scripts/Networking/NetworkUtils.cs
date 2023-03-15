using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Linq;

static class NetworkUtils
{
	// Made by dantheman213
	// https://gist.github.com/dantheman213/db3118bed76199186acf7be87af0c1c4
	// Searches after an IP from an avalibe Wi-Fi or Ethernet adapter
	public static IPAddress GetPrivateIP()
	{
		NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();

		foreach (NetworkInterface adapter in interfaces.Where(x => x.OperationalStatus == OperationalStatus.Up))
		{
			if (adapter.Name.ToLower() == "ethernet" || adapter.Name.ToLower() == "wi-fi")
			{

				IPInterfaceProperties props = adapter.GetIPProperties();
				UnicastIPAddressInformation result = props.UnicastAddresses.FirstOrDefault(
					x => x.Address.AddressFamily == AddressFamily.InterNetwork
				);

				if (result != null)
				{
					return result.Address;
				}
			}
		}

		return null;
	}
}