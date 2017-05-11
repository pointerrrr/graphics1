namespace Template
{
	//derive exercise 2 off of the base class Game
	class Exercise2 : Game
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
			screen.Print("Exercise 2", 2, 2, 0xffffff);
			screen.Line(2, 20, 160, 20, 0xff0000);

			// draw a square, pixel per pixel. The red is equal to the relative x, the green to the relative y
			for (int i = 0; i < 256; i++)
				for (int j = 0; j < 256; j++)
					screen.Plot(screen.width/2-127 + i, screen.height/2-127 + j, CreateRGB(i,j,0));
		}
	}

} // namespace Template