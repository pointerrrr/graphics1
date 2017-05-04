using System;
using System.IO;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Template
{

	class Exercise10 : Game
	{
		// member variables
		float[] vertexData;
		float screenscale = 8.0f;//width of screen
		float origX = 0.0f, origY = 0.0f;
		float a = 0.0f;
		float b = 0.0f;
		float c = 0.0f;
		float d = 0.0f;
		float depth = 0.5f;
		float size = 0.01f;
		float scale = 0.5f;
		int VBO;


		Surface map;
		float[,] h;


		// initialize
		public override void Init()
		{
			

			
			map = new Surface("../../assets/heightmap.png");
			h = new float[128, 128];
			for (int y = 0; y < 128; y++) for (int x = 0; x < 128; x++)
					h[x, y] = ( (float) ( map.pixels[x + y * 128] & 255 ) ) / 256;
			vertexData = new float[127 * 127 * 2 * 3 * 3];
			//vertexData = new float[127 * 127 * 4 * 3];
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
			screen.Print("Exercise 8", 2, 2, 0xffffff);
			screen.Print("A: " + a + " B: " + b + " C: " + c, 2, 30, 0xfffff);
			//d += (float) Math.PI / 90;
			/*screen.Line(TX(rotateX(x1, y1)), TY(rotateY(x1, y1)), TX(rotateX(x2, y2)), TY(rotateY(x2, y2)), 0xff0000);
			screen.Line(TX(rotateX(x2, y2)), TY(rotateY(x2, y2)), TX(rotateX(x3, y3)), TY(rotateY(x3, y3)), 0xff0000);
			screen.Line(TX(rotateX(x3, y3)), TY(rotateY(x3, y3)), TX(rotateX(x4, y4)), TY(rotateY(x4, y4)), 0xff0000);
			screen.Line(TX(rotateX(x4, y4)), TY(rotateY(x4, y4)), TX(rotateX(x1, y1)), TY(rotateY(x1, y1)), 0xff0000);*/
		}

		public override void RenderGL()
		{

			

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

			GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
			GL.DrawArrays(PrimitiveType.Triangles, 0, 127 * 127 * 2 * 3);
			

			/*GL.Begin(PrimitiveType.Quads);
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
			GL.End();*/
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
			x += screenscale / 2;
			x *= screen.width / screenscale;
			return (int) x;
		}

		public int TY(float y)
		{
			y += origY;
			y += screenscale / 2;
			y *= screen.width / screenscale;
			y = screen.height + ( screen.width - screen.height ) / 2 - y;
			return (int) y;
		}
	}

} // namespace Template