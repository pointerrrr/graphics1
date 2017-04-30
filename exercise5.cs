using System;
using System.IO;
using System.Windows.Forms;

namespace Template
{

	class Exercise5 : Game
	{
		// member variables
		/*float x1 = -1.0f, y1 = 1.0f;
		float x2 = 1.0f, y2 = 1.0f;
		float x3 = 1.0f, y3 = -1.0f;
		float x4 = -1.0f, y4 = -1.0f;*/
		float scaleX = 8.0f;//width of screen
		float scaleY = 5.0f;//height of screen
		float origX = 0.0f, origY = 0.0f;


		// initialize
		public override void Init()
		{
		}
		float a = (float) Math.PI / 4;
		// tick: renders one frame
		public override void Tick()
		{
			screen.Clear(0);
			screen.Print("Exercise 5", 2, 2, 0xffffff);
			//screen.Print("scaleX: " + scaleX + " scaleY: " + scaleY, 2, 30, 0xffffff);
			//screen.Print("X: " + origX + " Y: " + origY, 2, 50, 0xffffff);
			/*a += (float) Math.PI / 90;
			screen.Line(TX(rotateX(x1, y1)), TY(rotateY(x1, y1)), TX(rotateX(x2, y2)), TY(rotateY(x2, y2)), 0xff0000);
			screen.Line(TX(rotateX(x2, y2)), TY(rotateY(x2, y2)), TX(rotateX(x3, y3)), TY(rotateY(x3, y3)), 0xff0000);
			screen.Line(TX(rotateX(x3, y3)), TY(rotateY(x3, y3)), TX(rotateX(x4, y4)), TY(rotateY(x4, y4)), 0xff0000);
			screen.Line(TX(rotateX(x4, y4)), TY(rotateY(x4, y4)), TX(rotateX(x1, y1)), TY(rotateY(x1, y1)), 0xff0000);*/
			//screen.Line(screen.width / 2, 0, screen.width / 2, screen.height, 0xffffff);
			//screen.Print("1234567890", TX(-5), TY(0), 0xffffff);
			//screen.Plot(TX(0), TY(0), 0xffffff);
			Plot();
		}

		private void Plot()
		{
			float minX, minY, maxX, maxY;
			minX = -origX - scaleX / 2;
			maxX = -origX + scaleX / 2;
			minY = -origY - scaleY / 2;
			maxY = -origY + scaleY / 2;
			float curX = minX - scaleX / screen.width;
			float curY = retY(curX);
			while (curX <= maxX)
			{
				
				screen.Line(TX(curX), TY(curY),TX(curX + scaleX / screen.width), TY(retY(curX)) , 0xffffff);
				curY = retY(curX);
				curX += scaleX / screen.width;
			}
			screen.Line(TX(0), TY(minY), TX(0), TY(maxY), 0xffffff);
			screen.Line(TX(minX), TY(0), TX(maxX), TY(0), 0xffffff);
			DrawLabels(minX, minY, maxX, maxY);
		}

		private void DrawLabels(float minX, float minY, float maxX, float maxY)
		{
			minX = (int)Math.Floor(minX);
			while (minX <= maxX)
			{
				screen.Print(minX.ToString(), TX(minX), TY(0), 0xffffff);
				minX++;
			}
			minY = (int) Math.Floor(minY);
			while (minY <= maxY)
			{
				screen.Print(minY.ToString(), TX(0), TY(minY), 0xffffff);
				minY++;
			}
		}

		public float retY(float x)
		{
			return x * x;
		}

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

		private void Move(float x, float y)
		{
			origX += x;
			origY += y;
		}

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


		public float rotateX(float x, float y)
		{
			float rx = (float) ( x * Math.Cos(a) - y * Math.Sin(a) );
			return rx;
		}

		public float rotateY(float x, float y)
		{
			float ry = (float) ( x * Math.Sin(a) + y * Math.Cos(a) );
			return ry;
		}

		public int TX(float x)
		{
			x += origX;
			x += scaleX / 2;
			x *= screen.width / scaleX;
			return (int) x;
		}

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