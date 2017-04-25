using System;
using System.IO;

namespace Template {

	class Game
	{
		// member variables
		public Surface screen;

		// initialize
		public virtual void Init()
		{
		}

		// tick: renders one frame
		public virtual void Tick()
		{
			screen.Clear( 0 );
			screen.Print( "hello world", 2, 2, 0xffffff );
			screen.Line(2, 20, 160, 20, 0xff0000);
		}

		public static int CreateRGB(int red, int green, int blue)
		{
			return ( red << 16 ) + ( green << 8 ) + blue;
		}
	}

} // namespace Template