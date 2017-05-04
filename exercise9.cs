using System;
using System.IO;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Template
{

	class Exercise9 : Game
	{
		// member variables
		float[] vertexData, vertexColor;
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
		private int programID, vsID, fsID, attribute_vpos, attribute_vcol, uniform_mview, vbo_pos;


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
			vertexColor = new float[127 * 127 * 2 * 3 * 3];
			//vertexData = new float[127 * 127 * 4 * 3];
			int counter = 0;
			for (int i = 0; i < 127; i++)
				for (int j = 0; j < 127; j++)
				{
					float f = size * 2;
					float di = f * ( i - 63 );
					float dj = f * ( j - 63 );
					//vertex 1
					vertexColor[counter] = h[i, j];
					vertexData[counter++] = -size + di;
					vertexColor[counter] = 0.0f;
					vertexData[counter++] = -size + dj;
					vertexColor[counter] = 1.0f - h[i, j];
					vertexData[counter++] = ( -h[i, j] - depth ) * scale;
					//vertex2
					vertexColor[counter] = h[i + 1, j];
					vertexData[counter++] = size + di;
					vertexColor[counter] = 0.0f;
					vertexData[counter++] = -size + dj;
					vertexColor[counter] = 1.0f - h[i + 1, j];
					vertexData[counter++] = ( -h[i + 1, j] - depth ) * scale;
					//vertex3
					vertexColor[counter] = h[i + 1, j + 1];
					vertexData[counter++] = size + di;
					vertexColor[counter] = 0.0f;
					vertexData[counter++] = size + dj;
					vertexColor[counter] = 1.0f - h[i + 1, j + 1];
					vertexData[counter++] = ( -h[i + 1, j + 1] - depth ) * scale;
					//vertex4
					vertexColor[counter] = h[i, j + 1];
					vertexData[counter++] = -size + di;
					vertexColor[counter] = 0.0f;
					vertexData[counter++] = size + dj;
					vertexColor[counter] = 1.0f - h[i, j + 1];
					vertexData[counter++] = ( -h[i, j + 1] - depth ) * scale;
					//vertex5
					vertexColor[counter] = h[i + 1, j + 1];
					vertexData[counter++] = size + di;
					vertexColor[counter] = 0.0f;
					vertexData[counter++] = size + dj;
					vertexColor[counter] = 1.0f - h[i + 1, j + 1];
					vertexData[counter++] = ( -h[i + 1, j + 1] - depth ) * scale;
					//vertex6
					vertexColor[counter] = h[i, j];
					vertexData[counter++] = -size + di;
					vertexColor[counter] = 0.0f;
					vertexData[counter++] = -size + dj;
					vertexColor[counter] = 1.0f - h[i, j];
					vertexData[counter++] = ( -h[i, j] - depth ) * scale;
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

			programID = GL.CreateProgram();
			LoadShader("../../shaders/vs.glsl",
			 ShaderType.VertexShader, programID, out vsID);
			LoadShader("../../shaders/fs.glsl",
			 ShaderType.FragmentShader, programID, out fsID);
			GL.LinkProgram(programID);

			attribute_vpos = GL.GetAttribLocation(programID, "vPosition");
			attribute_vcol = GL.GetAttribLocation(programID, "vColor");
			uniform_mview = GL.GetUniformLocation(programID, "M");			vbo_pos = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_pos);
			GL.BufferData<float>(BufferTarget.ArrayBuffer,
				(IntPtr) ( vertexData.Length * 4 ),
				vertexData, BufferUsageHint.StaticDraw
			);
			GL.VertexAttribPointer(attribute_vpos, 3,
				VertexAttribPointerType.Float,
				false, 0, 0
			);

			vbo_pos = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_pos);
			GL.BufferData<float>(BufferTarget.ArrayBuffer,
				(IntPtr) ( vertexData.Length * 4 ),
				vertexColor, BufferUsageHint.StaticDraw
			);
			GL.VertexAttribPointer(attribute_vcol, 3,
				VertexAttribPointerType.Float,
				false, 0, 0
			);


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
			screen.Print("Exercise 9", 2, 2, 0xffffff);
			screen.Print("A: " + a + " B: " + b + " C: " + c, 2, 30, 0xfffff);
			a += (float)Math.PI/90;
			//d += (float) Math.PI / 90;
			/*screen.Line(TX(rotateX(x1, y1)), TY(rotateY(x1, y1)), TX(rotateX(x2, y2)), TY(rotateY(x2, y2)), 0xff0000);
			screen.Line(TX(rotateX(x2, y2)), TY(rotateY(x2, y2)), TX(rotateX(x3, y3)), TY(rotateY(x3, y3)), 0xff0000);
			screen.Line(TX(rotateX(x3, y3)), TY(rotateY(x3, y3)), TX(rotateX(x4, y4)), TY(rotateY(x4, y4)), 0xff0000);
			screen.Line(TX(rotateX(x4, y4)), TY(rotateY(x4, y4)), TX(rotateX(x1, y1)), TY(rotateY(x1, y1)), 0xff0000);*/
			

		}

		public override void RenderGL()
		{

			Matrix4 M;
			M = Matrix4.CreateFromAxisAngle(new Vector3(0, 0, 1), a);
			M *= Matrix4.CreateFromAxisAngle(new Vector3(1, 0, 0), 1.9f);
			M *= Matrix4.CreateTranslation(0, 0, -2);
			M *= Matrix4.CreatePerspectiveFieldOfView(1.6f, 1.3f, .1f, 1000);
			GL.UseProgram(programID);
			GL.UniformMatrix4(uniform_mview, false, ref M);
			GL.EnableVertexAttribArray(attribute_vpos);
			GL.EnableVertexAttribArray(attribute_vcol);
			GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
			GL.DrawArrays(PrimitiveType.Triangles, 0, 127 * 127 * 2 * 3);
			GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_pos);
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