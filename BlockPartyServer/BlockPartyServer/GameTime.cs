using System;

namespace BlockPartyServer
{
	class GameTime
	{
		public TimeSpan Elapsed;
		
		DateTime previousTime = DateTime.UtcNow;
		
		public void Update ()
		{
			Elapsed = DateTime.UtcNow - previousTime;
			previousTime = DateTime.UtcNow;
		}
	}
}
