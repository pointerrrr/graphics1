using System;

namespace Template
{

	class Exercise5 : Game
	{
		// member variables
		// dimensions of the screen
		float scaleX = 8.0f;
		float scaleY = 5.0f;
		//location of the center
		float origX = 0.0f, origY = 0.0f;
		//current rotation of the square
		float a = (float) Math.PI / 4;

		// initialize
		public override void Init()
		{
		}
		
		// tick: renders one frame
		public override void Tick()
		{
			screen.Clear(0);
			screen.Print("Exercise 5", 2, 2, 0xffffff);
			Plot();
		}


		private void Plot()
		{
			// bounds for drawing
			float minX, minY, maxX, maxY;
			minX = -origX - scaleX / 2;
			maxX = -origX + scaleX / 2;
			minY = -origY - scaleY / 2;
			maxY = -origY + scaleY / 2;
			// coordinates used for drawing graph
			float curX = minX - scaleX / screen.width;
			float curY = retY(curX);
			// loop while the x is within the bounds of the screen
			while (curX <= maxX)
			{
				// draw a line from the previous coordinates to the current coordinates (for continuity of the line)
				screen.Line(TX(curX), TY(curY),TX(curX + scaleX / screen.width), TY(retY(curX)) , 0xffffff);
				// update the current coordinates
				curY = retY(curX);
				curX += scaleX / screen.width;
			}
			// draw the axis lines
			screen.Line(TX(0), TY(minY), TX(0), TY(maxY), 0xffffff);
			screen.Line(TX(minX), TY(0), TX(maxX), TY(0), 0xffffff);
			DrawLabels(minX, minY, maxX, maxY);
		}

		// draw a small line on the axis' at each whole number and the corresponding coordinate
		private void DrawLabels(float minX, float minY, float maxX, float maxY)
		{
			minX = (int)Math.Floor(minX);
			// drawing the lines and numbers for X
			while (minX <= maxX)
			{
				screen.Line(TX(minX), TY(0)-8, TX(minX), TY(0)+8, 0xffffff);
				screen.Print(minX.ToString(), TX(minX) - minX.ToString().Length * 6, TY(0) + 4, 0xffffff);
				minX++;
			}
			// drawing the lines and numbers for Y
			minY = (int) Math.Floor(minY);
			while (minY <= maxY)
			{
				if (Math.Abs(minY - 0.1) > 0.125)
				{
					screen.Line(TX(0)-8, TY(minY), TX(0)+8, TY(minY), 0xffffff);
					screen.Print(minY.ToString(), TX(0) + 4, TY(minY) - 8, 0xffffff);
				}
				minY++;
			}
		}

		// what formula will be plotted
		public float retY(float x)
		{
			return x * x;
		}

		// transform the screen according to the key presses
		public override void Control(OpenTK.Input.KeyboardState keys)
		{
			currentKeyState = keys;
			if (NewKeyPress(OpenTK.Input.Key.Up))
			{
				Move(0, 1f);
			}
			if (NewKeyPress(OpenTK.Input.Key.Down))
			{
				Move(0, -1f);
			}
			if (NewKeyPress(OpenTK.Input.Key.Left))
			{
				Move(-1f, 0);
			}
			if (NewKeyPress(OpenTK.Input.Key.Right))
			{
				Move(1f, 0);
			}
			if (NewKeyPress(OpenTK.Input.Key.Z))
			{
				Zoom(false);
			}
			if (NewKeyPress(OpenTK.Input.Key.X))
			{
				Zoom(true);
			}
			base.Control(keys);
		}

		// move the origin
		private void Move(float x, float y)
		{
			origX += x;
			origY += y;
		}

		// zoom in/out
		private void Zoom(bool zoomIn)
		{
			if (zoomIn)
			{
				scaleX *= 2f;
				scaleY *= 2f;
			}
			else
			{
				scaleX /= 2f;
				scaleY /= 2f;
			}
		}

		//returns the rotated x value of the given point a degrees
		public float rotateX(float x, float y)
		{
			float rx = (float) ( x * Math.Cos(a) - y * Math.Sin(a) );
			return rx;
		}

		//returns the rotated y value of the given point a degrees
		public float rotateY(float x, float y)
		{
			float ry = (float) ( x * Math.Sin(a) + y * Math.Cos(a) );
			return ry;
		}

		//convert given x value to screen coordinates
		public int TX(float x)
		{
			x += origX;
			x += scaleX / 2;
			x *= screen.width / scaleX;
			return (int) x;
		}

		//convert given y value to screen coordinates
		public int TY(float y)
		{
			y += origY;
			y += scaleY / 2;
			y *= screen.height / scaleY;
			y = screen.height - y;
			return (int) y;
		}
	}

} // namespace Template