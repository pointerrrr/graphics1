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
		float a = 0.0f;// rotation of the island
		// size and scale of the heightmap
		float depth = 0.5f;
		float size = 0.01f;
		float scale = 0.5f;
		// heightmap
		Surface map;
		float[,] h;

		// initialize
		public override void Init()
		{
			map = new Surface("../../assets/heightmap.png");
			h = new float[128, 128];
			for (int y = 0; y < 128; y++) for (int x = 0; x < 128; x++)
					h[x, y] = ( (float) ( map.pixels[x + y * 128] & 255 ) ) / 256;
		}
		
		// tick: renders one frame
		public override void Tick()
		{
			screen.Clear(0);
			screen.Print("Exercise 6", 2, 2, 0xffffff);
			// rotate the heightmap 2 degrees
			a = (float)(( a + Math.PI / 90 ) % ( 2 * Math.PI ));
		}

		// render the GL
		public override void RenderGL()
		{
			// view matrix
			var M = Matrix4.CreatePerspectiveFieldOfView(1.6f, 1.3f, .1f, 1000);
			GL.LoadMatrix(ref M);
			GL.Translate(0, 0, -2);
			GL.Rotate(110, 1, 0, 0);
			GL.Rotate(a * 180 / Math.PI, 0, 0, 1);
			
			// draw every quad 
			GL.Begin(PrimitiveType.Quads);
			for (float i = 0; i < 127; i++)
				for (float j = 0; j < 127; j++)
				{
					float f = size * 2;
					float di = f * ( i - 63 );
					float dj = f * ( j - 63 );
					// first vertex
					GL.Color3(h[(int) i, (int) j], 0.0f, 1.0f - h[(int) i, (int) j]);
					GL.Vertex3(-size + di, -size + dj, ( -h[(int) i, (int) j] - depth ) * scale);
					// second vertex
					GL.Color3(h[(int) i+1, (int) j], 0.0f, 1.0f - h[(int) i + 1, (int) j]);
					GL.Vertex3(size + di, -size + dj, ( -h[(int) i +1, (int) j] - depth ) * scale);
					// third vertex
					GL.Color3(h[(int) i+1, (int) j+1], 0.0f, 1.0f - h[(int) i + 1, (int) j + 1]);
					GL.Vertex3(size + di, size + dj, ( -h[(int) i+1, (int) j +1] - depth ) * scale);
					// fourth vertex
					GL.Color3(h[(int) i, (int) j+1], 0.0f, 1.0f - h[(int) i, (int) j + 1]);
					GL.Vertex3(-size + di, size + dj, ( -h[(int) i , (int) j + 1] - depth ) * scale);
				}
			GL.End();
		}
	}

} // namespace Template