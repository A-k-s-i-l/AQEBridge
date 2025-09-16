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
			Console.WriteLine("Connected");

			foreach (var item in _client.Devices)
			{
				
				Console.WriteLine(item.Name);
			}
			while (true)
			{
				var delayTask = Task.Delay(150);
				var receiveTask = _udpClient.ReceiveAsync();

				var completed = await Task.WhenAny(delayTask, receiveTask);

				if (completed == delayTask)
				{
					SetVibration(0);
					Console.WriteLine("Timed out...");
					continue;
				}
				var r = await receiveTask;
				byte[] data = r.Buffer;
				float value = BitConverter.ToSingle(data, 0);
				Console.WriteLine($"Received: {value}");


				SetVibration(value);
			}

		}

		private void SetVibration(float value)
		{

			if (_client.Devices is null)
				return;
			foreach (var device in _client.Devices)
			{
				_ = device.VibrateAsync(value);

			}
		}

	}
}
