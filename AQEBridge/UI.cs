using Buttplug.Client;

namespace AQEBridge
{
	public class UI
	{
		public ConnectionStatus Status
		{
			get => _status;
			set
			{
				_status = value;
				if (value != ConnectionStatus.Connected && _timeStamp is null)
				{
					_timeStamp = DateTime.Now;
				}
				else if (value == ConnectionStatus.Connected)
				{
					_timeStamp = null;
				}
			}
		}
		public List<ButtplugClientDevice> Devices { get; set; }
		public float Strength { get; set; }

		private (DateTime, float)[] _history = new (DateTime, float)[10];

		private DateTime? _timeStamp;
		private ConnectionStatus _status;


		public UI()
		{
			Status = ConnectionStatus.Disconnected;
			Devices = new List<ButtplugClientDevice>();
		}

		public void Update()
		{
			Console.Clear();
		
			
			UpdateStatus();
			UpdateDevices();
			UpdateBar();
			UpdateHistory(Strength);
		
			Console.CursorVisible = false;
		}

		private void UpdateStatus()
		{
			if (Status == ConnectionStatus.Disconnected)
			{
				Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("Disconnected from intiface");
			}
			else if (Status == ConnectionStatus.Connected)
			{
				Console.ForegroundColor = ConsoleColor.Green; Console.WriteLine("All connected");
			}
			else
			{
				Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine("Game not connected/active");
			}
			/*	if(_timeStamp is not null)
				{
					Console.WriteLine((DateTime.Now-_timeStamp.Value).TotalSeconds);
				}*/
			Console.ResetColor();
			Console.WriteLine("------");
		}

		private void UpdateDevices()
		{
			Console.WriteLine();
			Console.WriteLine("Connected devices:");
			if (Devices.Count == 0)
			{
				Console.WriteLine("None");
				return;
			}
			foreach (var device in Devices)
			{
				Console.Write($"{device.Name}; ");
			}

		}

		private void UpdateBar()
		{
			Console.WriteLine();
			Console.WriteLine();
			Console.Write($"Current strength: {Strength}");

			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.WriteLine(Status==ConnectionStatus.GameNotConnected?"   Currently disabled":"");

			int segments = 20;
			int filledSegments = Math.Clamp((int)(Strength * segments), 0, 20);
			int emptySegments = segments - filledSegments;
			Console.ForegroundColor = ConsoleColor.Magenta;
			Console.Write("[");
			Console.ForegroundColor = ConsoleColor.DarkMagenta;
			Console.Write(new string('█', filledSegments));
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.Write(new string('░', emptySegments));
			Console.ForegroundColor = ConsoleColor.Magenta;
			Console.WriteLine("]");

			Console.ResetColor();
		}

		private void UpdateHistory(float strength)
		{
			if (Strength != _history[0].Item2)
			{
				for (int i = _history.Length - 1; i > 0; i--)
				{
					_history[i] = _history[i - 1];
				}
				_history[0] = (DateTime.Now, strength);
			}

			Console.WriteLine();
			Console.WriteLine("History (last 10):");
			foreach (var entry in _history)
			{
				if (entry.Item1 == default)
					continue;
				Console.WriteLine($"{entry.Item1:HH:mm:ss} - {entry.Item2}");
			}
		}
	
	}
}
public enum ConnectionStatus
{
	Disconnected,
	Connected,
	GameNotConnected,
}