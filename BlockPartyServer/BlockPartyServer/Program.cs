using System;

namespace BlockPartyServer
{
	public class Program
	{
		static readonly Game game = new Game ();

		public static void Main (string[] args)
		{
			game.Run ();
		}
	}
}

