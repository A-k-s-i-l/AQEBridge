using Buttplug.Client;
using System.Net;
using System.Net.Sockets;

namespace AQEBridge
{
	public class Client
	{
		private ButtplugClient _client;
		private UI _ui;
		ButtplugWebsocketConnector _connector;

		UdpClient _udpClient;
		IPEndPoint _endpoint;

		public Client(int port, UI ui)
		{

			Uri uri = new Uri("ws://localhost:12345");
			_connector = new ButtplugWebsocketConnector(uri);
			_client = new ButtplugClient("AQEBridge");

			_udpClient = new(port);
			_endpoint = new IPEndPoint(IPAddress.Any, port);
			this._ui = ui;
		}


		public async Task Start()
		{
			await _client.ConnectAsync(_connector);


			while (true)
			{
				var delayTask = Task.Delay(150);
				var receiveTask = _udpClient.ReceiveAsync();

				var completed = await Task.WhenAny(delayTask, receiveTask);

				if (completed == delayTask)
				{
					Handle(null, ConnectionStatus.GameNotConnected);

					continue;
				}
				var r = await receiveTask;
				byte[] data = r.Buffer;
				float value = BitConverter.ToSingle(data, 0);

				Handle(value, _client.Connected ? ConnectionStatus.Connected : ConnectionStatus.Disconnected);
			}

		}

		private void Handle(float? vibrationValue, ConnectionStatus status)
		{
			SetVibration(vibrationValue ?? 0);

			_ui.Strength = vibrationValue ?? _ui.Strength;
			_ui.Status = status;
			_ui.Devices = _client.Devices?.ToList() ?? new List<ButtplugClientDevice>();
			_ui.Update();
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
