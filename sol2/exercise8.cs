using System;
using System.IO;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Template
{

	class Exercise8 : Game
	{
		// member variables
		// size, scale and rotation of the heightmap
		float a = 0.0f;
		float depth = 0.5f;
		float size = 0.01f;
		float scale = 0.5f;
		// id of buffer
		int VBO;
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

			// initializing array
			vertexData = new float[127 * 127 * 2 * 3 * 3];
			int counter = 0;
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
			// linking the vertexData array to GL
			VBO = GL.GenBuffer();			GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
			GL.BufferData<float>(
				BufferTarget.ArrayBuffer,
				(IntPtr) ( vertexData.Length * 4 ),
				vertexData,
				BufferUsageHint.StaticDraw
			);
			GL.EnableClientState(ArrayCap.VertexArray);
			GL.VertexPointer(3, VertexPointerType.Float, 12, 0);
		}

		// tick: renders one frame
		public override void Tick()
		{
			screen.Clear(0);
			screen.Print("Exercise 8", 2, 2, 0xffffff);
			a = (float) ( ( a + Math.PI / 90 ) % ( 2 * Math.PI ) );
		}

		public override void RenderGL()
		{
			// matrix for perspective
			var M = Matrix4.CreatePerspectiveFieldOfView(1.6f, 1.3f, .1f, 1000);
			GL.LoadMatrix(ref M);
			// translating and rotating the world
			GL.Translate(0, 0, -2);
			GL.Rotate(110, 1, 0, 0);
			GL.Rotate(a * 180 / Math.PI, 0, 0, 1);
			
			// what array to draw
			GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
			// drawing the array
			GL.DrawArrays(PrimitiveType.Triangles, 0, 127 * 127 * 2 * 3);
		}
	}

} // namespace Template