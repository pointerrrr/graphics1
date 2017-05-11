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
		// rotation of the heightmap
		float a = 0.0f;
		// size and scale of the heightmap
		float depth = 0.5f;
		float size = 0.01f;
		float scale = 0.5f;
		// id's for programs, values, etc.
		int vsID, fsID, attribute_vpos, attribute_vcol, uniform_mview, vbo_pos, vbo_col;
		// arrays for final drawing
		float[] vertexData, colorData;
		// heightmap
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
			// initializing the arrays
			vertexData = new float[127 * 127 * 2 * 3 * 3];
			colorData = new float[127 * 127 * 2 * 3 * 3];
			int counter = 0;

			// filling the arrays by giving it these relative triangles: (0,0), (1,0), (1,1) and (0,1), (1,1), (0,0) and its color
			for (int i = 0; i < 127; i++)
				for (int j = 0; j < 127; j++)
				{
					float f = size * 2;
					float di = f * ( i - 63 );
					float dj = f * ( j - 63 );
					//vertex 1
					colorData[counter] = h[i, j];
					vertexData[counter++] = -size + di;
					colorData[counter] = 0.0f;
					vertexData[counter++] = -size + dj;
					colorData[counter] = 1.0f - h[i, j];
					vertexData[counter++] = ( -h[i, j] - depth ) * scale;
					//vertex2
					colorData[counter] = h[i + 1, j];
					vertexData[counter++] = size + di;
					colorData[counter] = 0.0f;
					vertexData[counter++] = -size + dj;
					colorData[counter] = 1.0f - h[i + 1, j];
					vertexData[counter++] = ( -h[i + 1, j] - depth ) * scale;
					//vertex3
					colorData[counter] = h[i + 1, j + 1];
					vertexData[counter++] = size + di;
					colorData[counter] = 0.0f;
					vertexData[counter++] = size + dj;
					colorData[counter] = 1.0f - h[i + 1, j + 1];
					vertexData[counter++] = ( -h[i + 1, j + 1] - depth ) * scale;
					//vertex4
					colorData[counter] = h[i, j + 1];
					vertexData[counter++] = -size + di;
					colorData[counter] = 0.0f;
					vertexData[counter++] = size + dj;
					colorData[counter] = 1.0f - h[i, j + 1];
					vertexData[counter++] = ( -h[i, j + 1] - depth ) * scale;
					//vertex5
					colorData[counter] = h[i + 1, j + 1];
					vertexData[counter++] = size + di;
					colorData[counter] = 0.0f;
					vertexData[counter++] = size + dj;
					colorData[counter] = 1.0f - h[i + 1, j + 1];
					vertexData[counter++] = ( -h[i + 1, j + 1] - depth ) * scale;
					//vertex6
					colorData[counter] = h[i, j];
					vertexData[counter++] = -size + di;
					colorData[counter] = 0.0f;
					vertexData[counter++] = -size + dj;
					colorData[counter] = 1.0f - h[i, j];
					vertexData[counter++] = ( -h[i, j] - depth ) * scale;
				}

			// creating the shader program
			programID = GL.CreateProgram();

			// loading the shaders
			LoadShader("../../shaders/vs.glsl",
			 ShaderType.VertexShader, programID, out vsID);
			LoadShader("../../shaders/fs.glsl",
			 ShaderType.FragmentShader, programID, out fsID);
			
			// linking the program
			GL.LinkProgram(programID);

			// getting the shader variable id's for later use
			attribute_vpos = GL.GetAttribLocation(programID, "vPosition");
			attribute_vcol = GL.GetAttribLocation(programID, "vColor");
			uniform_mview = GL.GetUniformLocation(programID, "M");			// linking the position array			vbo_pos = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_pos);
			GL.BufferData<float>(BufferTarget.ArrayBuffer,
				(IntPtr) ( vertexData.Length * 4 ),
				vertexData, BufferUsageHint.StaticDraw
			);
			GL.VertexAttribPointer(attribute_vpos, 3,
				VertexAttribPointerType.Float,
				false, 0, 0
			);
			GL.EnableClientState(ArrayCap.VertexArray);
			GL.VertexPointer(3, VertexPointerType.Float, 12, 0);

			// linking the color array
			vbo_col = GL.GenBuffer();			
			GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_col);
			GL.BufferData<float>(BufferTarget.ArrayBuffer,
				(IntPtr) ( colorData.Length * 4 ),
				colorData, BufferUsageHint.StaticDraw
			);
			GL.VertexAttribPointer(attribute_vcol, 3,
				VertexAttribPointerType.Float,
				false, 0, 0
			);

			// telling GL to use the program
			GL.UseProgram(programID);
		}
		
		// tick: renders one frame
		public override void Tick()
		{
			screen.Clear(0);
			screen.Print("Exercise 9", 2, 2, 0xffffff);
			a = (float) ( ( a + Math.PI / 90 ) % ( 2 * Math.PI ) );
		}

		public override void RenderGL()
		{
			// view matrix
			Matrix4 M;
			M = Matrix4.CreateFromAxisAngle(new Vector3(0, 0, 1), a);
			M *= Matrix4.CreateFromAxisAngle(new Vector3(1, 0, 0), 1.9f);
			M *= Matrix4.CreateTranslation(0, 0, -2);
			M *= Matrix4.CreatePerspectiveFieldOfView(1.6f, 1.3f, .1f, 1000);
			
			// linking the uniform matrix to the shader
			GL.UniformMatrix4(uniform_mview, false, ref M);

			// enable the array streams
			GL.EnableVertexAttribArray(attribute_vpos);
			GL.EnableVertexAttribArray(attribute_vcol);
			
			// draw the arrays
			GL.DrawArrays(PrimitiveType.Triangles, 0, 127 * 127 * 2 * 3);
		}
	}

} // namespace Template