using System;
using System.IO;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Template
{

	class Exercise7 : Game
	{
		// member variables
		float a = 0.0f;
		float depth = 0.5f;
		float size = 0.01f;
		float scale = 0.5f;
		float[] vertexData;
		float[,] h;
		Surface map;
		
		// initialize
		public override void Init()
		{
			// heightmap
			map = new Surface("../../assets/heightmap.png");
			h = new float[128, 128];
			for (int y = 0; y < 128; y++) for (int x = 0; x < 128; x++)
					h[x, y] = ( (float) ( map.pixels[x + y * 128] & 255 ) ) / 256;
			// initialize the array
			vertexData = new float[127 * 127 * 2 * 3 * 3];
			// keeping track at what vertex we are
			int counter = 0;
			// fill the array by giving it these relative triangles: (0,0), (1,0), (1,1) and (0,1), (1,1), (0,0)
			for (int i = 0; i < 127; i++)
				for (int j = 0; j < 127; j++)
				{
					float f = size * 2;
					float di = f * ( i - 63 );
					float dj = f * ( j - 63 );
					//vertex 1
					vertexData[counter++] = -size + di;
					vertexData[counter++] = -size + dj;
					vertexData[counter++] = ( -h[(int) i, (int) j] - depth ) * scale;
					//vertex2
					vertexData[counter++] = size + di;
					vertexData[counter++] = -size + dj;
					vertexData[counter++] = ( -h[(int) i + 1, (int) j] - depth ) * scale;
					//vertex3
					vertexData[counter++] = size + di;
					vertexData[counter++] = size + dj;
					vertexData[counter++] = ( -h[(int) i + 1, (int) j + 1] - depth ) * scale;
					//vertex4
					vertexData[counter++] = -size + di;
					vertexData[counter++] = size + dj;
					vertexData[counter++] = ( -h[(int) i, (int) j + 1] - depth ) * scale;
					//vertex5
					vertexData[counter++] = size + di;
					vertexData[counter++] = size + dj;
					vertexData[counter++] = ( -h[(int) i + 1, (int) j + 1] - depth ) * scale;
					//vertex6
					vertexData[counter++] = -size + di;
					vertexData[counter++] = -size + dj;
					vertexData[counter++] = ( -h[(int) i, (int) j] - depth ) * scale;
				}
		}
		
		// tick: renders one frame
		public override void Tick()
		{
			screen.Clear(0);
			screen.Print("Exercise 7", 2, 2, 0xffffff);
			a = (float) ( ( a + Math.PI / 90 ) % ( 2 * Math.PI ) );
		}

		public override void RenderGL()
		{
			// perspective matrix
			var M = Matrix4.CreatePerspectiveFieldOfView(1.6f, 1.3f, .1f, 1000);
			GL.LoadMatrix(ref M);
			GL.Translate(0, 0, -2);
			GL.Rotate(110, 1, 0, 0);
			GL.Rotate(a * 180 / Math.PI, 0, 0, 1);
			
			// begin drawing the quads
			GL.Begin(PrimitiveType.Quads);
			for (float i = 0; i < 127; i++)
				for (float j = 0; j < 127; j++)
				{
					float f = size * 2;
					float di = f * ( i - 63 );
					float dj = f * ( j - 63 );
					GL.Color3(h[(int) i, (int) j], 0.0f, 1.0f - h[(int) i, (int) j]);
					GL.Vertex3(-size + di, -size + dj, ( -h[(int) i, (int) j] - depth ) * scale);

					GL.Color3(h[(int) i + 1, (int) j], 0.0f, 1.0f - h[(int) i + 1, (int) j]);
					GL.Vertex3(size + di, -size + dj, ( -h[(int) i + 1, (int) j] - depth ) * scale);

					GL.Color3(h[(int) i + 1, (int) j + 1], 0.0f, 1.0f - h[(int) i + 1, (int) j + 1]);
					GL.Vertex3(size + di, size + dj, ( -h[(int) i + 1, (int) j + 1] - depth ) * scale);

					GL.Color3(h[(int) i, (int) j + 1], 0.0f, 1.0f - h[(int) i, (int) j + 1]);
					GL.Vertex3(-size + di, size + dj, ( -h[(int) i, (int) j + 1] - depth ) * scale);
				}
			GL.End();
		}
	}

} // namespace Template