using System;
using System.IO;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Template
{

	class Exercise6 : Game
	{
		// member variables
		float scale = 8.0f;//width of screen
		float origX = 0.0f, origY = 0.0f;
		float a = 0.0f;
		float b = 0.0f;
		float c = 0.0f;
		float d = 0.0f;
		// initialize
		public override void Init()
		{
		}

		public override void Control(KeyboardState keys)
		{
			currentKeyState = keys;
			if (NewKeyPress(OpenTK.Input.Key.Left))
			{
				a += (float) Math.PI / 90;
			}
			if (NewKeyPress(OpenTK.Input.Key.Right))
			{
				a -= (float) Math.PI / 90;
			}
			if (NewKeyPress(OpenTK.Input.Key.Up))
			{
				b += (float) Math.PI / 90;
			}
			if (NewKeyPress(OpenTK.Input.Key.Down))
			{
				b -= (float) Math.PI / 90;
			}

			if (NewKeyPress(OpenTK.Input.Key.Z))
			{
				c += (float) Math.PI / 90;
			}
			if (NewKeyPress(OpenTK.Input.Key.X))
			{
				c -= (float) Math.PI / 90;
			}
			//base.Control(keys);

		}

		// tick: renders one frame
		public override void Tick()
		{
			screen.Clear(0);
			screen.Print("Exercise 6", 2, 2, 0xffffff);
			screen.Print("A: " + a + " B: " + b + " C: " + c, 2, 30, 0xfffff);
			//d += (float) Math.PI / 90;
			/*screen.Line(TX(rotateX(x1, y1)), TY(rotateY(x1, y1)), TX(rotateX(x2, y2)), TY(rotateY(x2, y2)), 0xff0000);
			screen.Line(TX(rotateX(x2, y2)), TY(rotateY(x2, y2)), TX(rotateX(x3, y3)), TY(rotateY(x3, y3)), 0xff0000);
			screen.Line(TX(rotateX(x3, y3)), TY(rotateY(x3, y3)), TX(rotateX(x4, y4)), TY(rotateY(x4, y4)), 0xff0000);
			screen.Line(TX(rotateX(x4, y4)), TY(rotateY(x4, y4)), TX(rotateX(x1, y1)), TY(rotateY(x1, y1)), 0xff0000);*/
		}

		public override void RenderGL()
		{
			Surface map;
			float[,] h;			map = new Surface("../../assets/heightmap.png");
			h = new float[128, 128];
			for (int y = 0; y < 128; y++) for (int x = 0; x < 128; x++)
					h[x, y] = ( (float) ( map.pixels[x + y * 128] & 255 ) ) / 256;

			var M = Matrix4.CreatePerspectiveFieldOfView(1.6f, 1.3f, .1f, 1000);
			GL.LoadMatrix(ref M);
			GL.Translate(0, 0, -2);
			GL.Rotate(110, 1, 0, 0);
			GL.Rotate(d * 180 / Math.PI, 0, 0, 1);
			//GL.Translate(0, 0, -2);
			//GL.Rotate(20, -1, 0, 0);
			GL.Rotate(a * 180 / Math.PI, 0, 0, 1);
			GL.Rotate(b * 180 / Math.PI, 0, 1, 0);
			GL.Rotate(c * 180 / Math.PI, 1, 0, 0);
			float depth = 0.5f;
			float size = 0.01f;
			float scale = 0.5f;
			GL.Begin(PrimitiveType.Quads);
			for (float i = 1; i < 126; i++)
				for (float j = 1; j < 126; j++)
				{
					GL.Color3(h[(int) i, (int) j], 0.0f, 1.0f - h[(int) i, (int) j]);
					
					float f = size * 2;
					float di = f * ( i - 63 );
					float dj = f * ( j - 63 );
					GL.Color3(1.0f, 0.0f, 0.0f);
					GL.Vertex3(-size + di, size + dj, ( -h[(int) i-1, (int) j-1] - depth ) * scale);
					
					GL.Vertex3(size + di, size + dj, ( -h[(int) i, (int) j-1] - depth ) * scale);
					GL.Color3(0.0f, 0.0f, 1.0f);
					GL.Vertex3(size + di, -size + dj, ( -h[(int) i-1, (int) j] - depth ) * scale);
					
					GL.Vertex3(-size + di, -size + dj, ( -h[(int) i, (int) j] - depth ) * scale);
					GL.Color3(1.0f, 1.0f, 1.0f);




				}
			GL.End();
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
			x += scale / 2;
			x *= screen.width / scale;
			return (int) x;
		}

		public int TY(float y)
		{
			y += origY;
			y += scale / 2;
			y *= screen.width / scale;
			y = screen.height + ( screen.width - screen.height ) / 2 - y;
			return (int) y;
		}
	}

} // namespace Template