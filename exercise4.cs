using System;
using System.IO;
using System.Windows.Forms;

namespace Template
{

	class Exercise4 : Game
	{
		// member variables
		float x1 = -1.0f, y1 = 1.0f;
		float x2 = 1.0f, y2 = 1.0f;
		float x3 = 1.0f, y3 = -1.0f;
		float x4 = -1.0f, y4 = -1.0f;
		float scale = 8.0f;//width of screen
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
			screen.Print("Exercise 4", 2, 2, 0xffffff);
			a += (float) Math.PI / 90;
			screen.Line(TX(rotateX(x1, y1)), TY(rotateY(x1, y1)), TX(rotateX(x2, y2)), TY(rotateY(x2, y2)), 0xff0000);
			screen.Line(TX(rotateX(x2, y2)), TY(rotateY(x2, y2)), TX(rotateX(x3, y3)), TY(rotateY(x3, y3)), 0xff0000);
			screen.Line(TX(rotateX(x3, y3)), TY(rotateY(x3, y3)), TX(rotateX(x4, y4)), TY(rotateY(x4, y4)), 0xff0000);
			screen.Line(TX(rotateX(x4, y4)), TY(rotateY(x4, y4)), TX(rotateX(x1, y1)), TY(rotateY(x1, y1)), 0xff0000);
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
			x += scale/2;
			x *= screen.width/scale;
			return (int) x;
		}

		public int TY(float y)
		{
			y += origY;
			y += scale/2;
			y *= screen.width/scale;
			y = screen.height + (screen.width - screen.height) / 2 - y;
			return (int) y;
		}
	}

} // namespace Template