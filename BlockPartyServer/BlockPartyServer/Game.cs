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
			Game,
			Results
		}

		GameState state;

		TimeSpan lobbyElapsed;
		TimeSpan lobbyDuration = TimeSpan.FromSeconds (15);

		TimeSpan gameElapsed;
		TimeSpan gameDuration = TimeSpan.FromSeconds (120);

		TimeSpan resultsElapsed;
		TimeSpan resultsDuration = TimeSpan.FromSeconds (15);

		Timer updateTimer;
		const int updatesPerSecond = 1;

		GameTime gameTime = new GameTime ();
		
		NetworkingManager networkingManager = new NetworkingManager ();
		UserManager userManager = new UserManager ();
		
		Dictionary<string, int> gameResults = new Dictionary<string, int> ();
		bool processedResults;

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
				if (!userManager.Users.ContainsKey (e.Sender.Client.RemoteEndPoint.ToString ())) {
					userManager.Users.Add (e.Sender.Client.RemoteEndPoint.ToString (), new User ((string)e.Message.Content, e.Sender));
				}
				networkingManager.Send (userManager.Users [e.Sender.Client.RemoteEndPoint.ToString ()].Client, CreateGameStateMessage ());
				break;

			case NetworkMessage.MessageType.ClientSignInUser:
				userManager.Users [e.Sender.Client.RemoteEndPoint.ToString ()].Name = (string)e.Message.Content;
				break;

			case NetworkMessage.MessageType.ClientResults:
				if (!gameResults.ContainsKey (userManager.Users [e.Sender.Client.RemoteEndPoint.ToString ()].Name)) {
					gameResults.Add (userManager.Users [e.Sender.Client.RemoteEndPoint.ToString ()].Name, (int)e.Message.Content);
				}
				break;
			}
		}

		NetworkMessage CreateGameStateMessage ()
		{
			float timeRemaining = 0.0f;
			
			if (state == GameState.Game) {
				timeRemaining = (float)(gameDuration.TotalSeconds - gameElapsed.TotalSeconds);
			}
			
			if (state == GameState.Lobby) {
				timeRemaining = (float)(lobbyDuration.TotalSeconds - lobbyElapsed.TotalSeconds);
			}

			if (state == GameState.Results) {
				timeRemaining = (float)(resultsDuration.TotalSeconds - resultsElapsed.TotalSeconds);
			}
			
			ServerGameStateContent content = new ServerGameStateContent (state.ToString (), timeRemaining);
			
			NetworkMessage message = new NetworkMessage (NetworkMessage.MessageType.ServerGameState, content);
			Console.WriteLine ("Created network message: type=ServerGameState, content=" + content.ToString ());

			return message;
		}

		void SetGameState (GameState state)
		{
			Console.WriteLine ("Setting game state to " + state.ToString ());
			this.state = state;
			
			switch (state) {
			case GameState.Lobby:
				lobbyElapsed = TimeSpan.Zero;
				processedResults = false;
				networkingManager.Broadcast (CreateGameStateMessage ());														
				break;

			case GameState.Game:
				gameElapsed = TimeSpan.Zero;
				networkingManager.Broadcast (CreateGameStateMessage ());
				break;
				
			case GameState.Results:
				resultsElapsed = TimeSpan.Zero;
				gameResults.Clear ();
				networkingManager.Broadcast (CreateGameStateMessage ());
				break;
			}
		}
		
		void Update (object sender, ElapsedEventArgs e)
		{
			gameTime.Update ();
			
			switch (state) {
			case GameState.Lobby:
				if (!processedResults) {
					Console.WriteLine ("gameResults.Count=" + gameResults.Count);
					
					List<KeyValuePair<string, int>> sortedGameResults = gameResults.ToList ();
					sortedGameResults.Sort ((firstPair, nextPair) => {
						return firstPair.Value.CompareTo (nextPair) * -1; });
					
					if (sortedGameResults.Count > 0) {
						Console.WriteLine ("Game winner is " + sortedGameResults [0].Key + " with score " + sortedGameResults [0].Value);
						
						networkingManager.Broadcast (new NetworkMessage (NetworkMessage.MessageType.ServerLeaderboard, sortedGameResults));
					}
					
					processedResults = true;
				}

				lobbyElapsed += gameTime.Elapsed;

				if (lobbyElapsed >= lobbyDuration) {
					SetGameState (GameState.Game);
				}
				break;
				
			case GameState.Game:
				gameElapsed += gameTime.Elapsed;
				
				if (gameElapsed >= gameDuration) {
					SetGameState (GameState.Results);
				}
				break;

			case GameState.Results:
				resultsElapsed += gameTime.Elapsed;

				if (resultsElapsed >= resultsDuration) {
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
