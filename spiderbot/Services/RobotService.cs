using System;
using WebSocket.Portable;
using System.Threading.Tasks;
using System.Diagnostics;
using Sockets.Plugin;

namespace spiderbot
{
	

	public class RobotService
	{

		private static readonly Lazy<RobotService> lazy =
			new Lazy<RobotService>(() => new RobotService());

		public static RobotService Instance { get { return lazy.Value; } }

		WebSocketClient client;
		public String host;

		public RobotService ()
		{
			this.host = host;
		}

		public async Task connect(){
			client = new WebSocketClient ();


			client.Opened += () => {
				Debug.WriteLine("Connected to Robot");
			};

			client.MessageReceived += (obj) => {
				Debug.WriteLine("Message recieved: " + obj);
			};

			await client.OpenAsync (host);
		}

		public async Task GoForward(){
			await connect ();
			await client.SendAsync ("command walkY 130");
		}


	}
}

