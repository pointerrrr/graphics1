﻿namespace Template
{

	class Exercise1 : Game
	{
		// member variables
		
		// initialize

		public override void Init()
		{
		}

		// tick: renders one frame
		public override void Tick()
		{
			screen.Clear(0);
			screen.Print("Exercise 1", 2, 2, 0xffffff);
			screen.Line(2, 20, 160, 20, 0xff0000);

			// draw 256 lines with the relative x value as blue value
			for (int i = 0; i < 256; i++)
				screen.Line(screen.width / 2 + 127 - i, screen.height / 2 - 127, screen.width / 2 + 127 - i, screen.height / 2 + 127, CreateRGB(0, 0, 255-i));
		}
	}

} // namespace Template