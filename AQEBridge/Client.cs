using Buttplug.Client;
using System.Net;
using System.Net.Sockets;

namespace AQEBridge
{
	public class Client
	{
		private ButtplugClient _client;
		ButtplugWebsocketConnector _connector;

		UdpClient _udpClient;
		IPEndPoint _endpoint;

		public Client(int port)
		{

			Uri uri = new Uri("ws://localhost:12345");
			_connector = new ButtplugWebsocketConnector(uri);
			_client = new ButtplugClient("AQEBridge");

			_udpClient = new(port);
			_endpoint = new IPEndPoint(IPAddress.Any, port);
		}

		public async Task Start()
		{
			await _client.ConnectAsync(_connector);
			
			foreach (var item in _client.Devices)
			{
				Console.WriteLine(item.Name);
			}
			while (true)
			{
				byte[] data =  _udpClient.Receive(ref _endpoint);
				float value = BitConverter.ToSingle(data, 0);
				Console.WriteLine($"Received: {value}");


				if(_client.Devices is null)
					continue;
				foreach (var device in _client.Devices)
				{
					_= device.VibrateAsync(value);
					
				}
			}

		}

	}
}
