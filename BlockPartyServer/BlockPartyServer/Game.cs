using System;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using BlockPartyShared;

namespace BlockPartyServer
{
	public class Game
	{
		enum GameState
		{
			Lobby,
			Gameplay,
			Postgame,
		}

		GameState state;

		TimeSpan lobbyElapsed;
		TimeSpan lobbyDuration = TimeSpan.FromSeconds (10);

		TimeSpan gameplayElapsed;
		TimeSpan gameplayDuration = TimeSpan.FromSeconds (10);

		TimeSpan postgameElapsed;
		TimeSpan postgameDuration = TimeSpan.FromSeconds (10);

		Timer updateTimer;
		const int updatesPerSecond = 1;

		GameTime gameTime = new GameTime ();
		
		NetworkingManager networkingManager = new NetworkingManager ();
		UserManager userManager = new UserManager ();
		
		Dictionary<string, int> gameResults = new Dictionary<string, int> ();
		
		public Game ()
		{
			networkingManager.MessageReceived += networkingManager_MessageReceived;
			
			updateTimer = new Timer (1000.0f / updatesPerSecond);
			updateTimer.Elapsed += Update;
			updateTimer.Start ();
			
			SetGameState (GameState.Lobby);
		}
		
		void networkingManager_MessageReceived (object sender, MessageReceivedEventArgs e)
		{
			switch (e.Message.Type) {
			case NetworkMessage.MessageType.ClientCreateUser:
				userManager.Users.Add ((string)e.Message.Content, new User ((string)e.Message.Content, e.Sender));
				break;
			case NetworkMessage.MessageType.ClientResults:
				gameResults.Add (userManager.GetUserByTcpClient (e.Sender).Name, (int)e.Message.Content);
				break;
			}
		}
		
		void SetGameState (GameState state)
		{
			Console.WriteLine ("Setting game state to " + state.ToString ());
			this.state = state;
			
			NetworkMessage message = new NetworkMessage (NetworkMessage.MessageType.ServerGameState, state.ToString ());
			networkingManager.Broadcast (message);
			
			switch (state) {
			case GameState.Gameplay:
				gameplayElapsed = TimeSpan.Zero;
				gameResults.Clear ();
				break;
				
			case GameState.Postgame:
				postgameElapsed = TimeSpan.Zero;
				break;
				
			case GameState.Lobby:
				lobbyElapsed = TimeSpan.Zero;
				
				Console.WriteLine ("gameResults.Count=" + gameResults.Count);
				List<KeyValuePair<string, int>> sortedGameResults = gameResults.ToList ();
				sortedGameResults.Sort ((firstPair, nextPair) => {
					return firstPair.Value.CompareTo (nextPair) * -1; });
				Console.WriteLine ("sortedGameResults.Count=" + sortedGameResults.Count);
				if (sortedGameResults.Count > 0) {
					Console.WriteLine ("Game winner is " + sortedGameResults [0].Key + " with score " + sortedGameResults [0].Value);
					
					networkingManager.Broadcast (new NetworkMessage (NetworkMessage.MessageType.ServerLeaderboard, sortedGameResults));
				}
				
				break;
			}
		}
		
		void Update (object sender, ElapsedEventArgs e)
		{
			gameTime.Update ();
			
			switch (state) {
			case GameState.Lobby:
				lobbyElapsed += gameTime.Elapsed;
				
				if (lobbyElapsed >= lobbyDuration) {
					SetGameState (GameState.Gameplay);
				}
				break;
				
			case GameState.Gameplay:
				gameplayElapsed += gameTime.Elapsed;
				
				if (gameplayElapsed >= gameplayDuration) {
					SetGameState (GameState.Postgame);
				}
				break;
				
			case GameState.Postgame:
				postgameElapsed += gameTime.Elapsed;
				
				if (postgameElapsed >= postgameDuration) {
					SetGameState (GameState.Lobby);
				}
				break;
			}
		}
		
		public void Run ()
		{
			while (true) {
			}
		}
	}
}
