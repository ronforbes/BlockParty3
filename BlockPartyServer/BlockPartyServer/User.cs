using System.Net.Sockets;

namespace BlockPartyServer
{
	public class User
	{
		public string Name;
		public TcpClient Client;
		
		public User (string name, TcpClient client)
		{
			Name = name;
			Client = client;
		}
	}
}
