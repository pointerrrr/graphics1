using System;
using System.IO;

namespace Template
{

	class Exercise4 : Game
	{
		// member variables
		// rotation of the square
		float a = (float) Math.PI / 4;
		// coordinates of the square
		float x1 = -1.0f, y1 = 1.0f;
		float x2 = 1.0f, y2 = 1.0f;
		float x3 = 1.0f, y3 = -1.0f;
		float x4 = -1.0f, y4 = -1.0f;
		// dimensions of the screen
		float scaleX = 8.0f;
		float scaleY = 5.0f;
		// center
		float origX = 0.0f, origY = 0.0f;

		// initialize
		public override void Init()
		{
		}
		
		// tick: renders one frame
		public override void Tick()
		{
			screen.Clear(0);
			screen.Print("Exercise 4", 2, 2, 0xffffff);
			a += (float) Math.PI / 90;
			screen.Line(TX(rotateX(x1, y1)), TY(rotateY(x1, y1)), TX(rotateX(x2, y2)), TY(rotateY(x2, y2)), 0xff0000);
			screen.Line(TX(rotateX(x2, y2)), TY(rotateY(x2, y2)), TX(rotateX(x3, y3)), TY(rotateY(x3, y3)), 0xff0000);
			screen.Line(TX(rotateX(x3, y3)), TY(rotateY(x3, y3)), TX(rotateX(x4, y4)), TY(rotateY(x4, y4)), 0xff0000);
			screen.Line(TX(rotateX(x4, y4)), TY(rotateY(x4, y4)), TX(rotateX(x1, y1)), TY(rotateY(x1, y1)), 0xff0000);
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