using System;
using Mono.Nat;
using WaveBox.DataModel.Singletons;

namespace WaveBox
{
	public enum NatStatus
	{
		NotInitialized            = 0,
		WaitingForDevice          = 1,
		DeviceFound               = 2,
		PortForwardedSuccessfully = 3,
		PortForwardingFailed      = 4
	}

	public class Nat
	{
		public NatStatus Status { get; set; }

		public void Start()
		{
			this.Status = NatStatus.WaitingForDevice;

			Console.WriteLine("[Nat] Starting NAT process");

			// Hook into the events so you know when a router has been detected or has gone offline
			NatUtility.DeviceFound += DeviceFound;
			NatUtility.DeviceLost += DeviceLost;
			NatUtility.UnhandledException += UnhandledException;
			
			// Start searching for upnp enabled routers
			NatUtility.StartDiscovery();
		}
		
		private void DeviceFound(object sender, DeviceEventArgs args)
		{
			this.Status = NatStatus.DeviceFound;

			// This is the upnp enabled router
			INatDevice device = args.Device;
			
			// Create a mapping to forward external port to local port
			device.CreatePortMap(new Mapping(Protocol.Tcp, Settings.Port, Settings.Port));

			Console.WriteLine("[Nat] Device Found");

			this.Status = NatStatus.PortForwardedSuccessfully;
			
			/*// Retrieve the details for the port map for external port 3000
			Mapping m = device.GetSpecificMapping(Protocol.Tcp, 3000);
			
			// Get all the port mappings on the device and delete them
			foreach (Mapping mp in device.GetAllMappings())
				device.DeletePortMap(mp);
			
			// Get the external IP address
			IPAddress externalIP = device.GetExternalIP();*/
		}
		
		private void DeviceLost(object sender, DeviceEventArgs args)
		{
			this.Status = NatStatus.PortForwardingFailed;

			INatDevice device = args.Device;
			
			Console.WriteLine("[Nat] Device Lost");
			Console.WriteLine("[Nat] Type: {0}", device.GetType().Name);
		}

		private void UnhandledException(object sender, UnhandledExceptionEventArgs args)
		{
			this.Status = NatStatus.PortForwardingFailed;

			Console.WriteLine("[Nat] Unhandled exception: {0}", args);
		}
	}
}
