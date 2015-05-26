using System;
using System.Net.Sockets;

namespace BlockPartyShared
{
	[Serializable]
	public class NetworkMessage
	{
		public enum MessageType
		{
			ServerGameState,
			ServerLeaderboard,
			ClientCreateUser,
			ClientSignInUser,
			ClientResults
		}
		
		public MessageType Type;
		public object Content;
		
		public NetworkMessage (MessageType type, object content)
		{
			Type = type;
			Content = content;
		}
		
		public override string ToString ()
		{
			return string.Format ("[NetworkMessage: Type={0}, Content={1}]", Type.ToString (), Content.ToString ());
		}
	}
	
	public class MessageReceivedEventArgs : EventArgs
	{
		public NetworkMessage Message { get; set; }
		
		public TcpClient Sender { get; set; }
	}

	[Serializable]
	public class ServerGameStateContent
	{
		public string GameState;
		public float TimeRemaining;

		public ServerGameStateContent (string gameState, float timeRemaining)
		{
			GameState = gameState;
			TimeRemaining = timeRemaining;
		}

		public override string ToString ()
		{
			return string.Format ("[ServerGameStateContent: GameStatae={0}, TimeRemaining={1}", GameState.ToString (), TimeRemaining.ToString ());
		}
	}
}
