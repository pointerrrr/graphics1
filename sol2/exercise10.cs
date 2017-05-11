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
		// rotation and size of the heightmap
		float a = 0.0f;
		float depth = 0.5f;
		float size = 0.01f;
		float scale = 0.5f;
		// id's of values, programs etc
		int vsID, fsID, attribute_vpos, attribute_vcol, attribute_vnor, uniform_mview, uniform_mvpiew, uniform_lightpos, vbo_pos, vbo_col, vbo_nor;
		float[] vertexData, colorData, vertexNormal;
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
			// initialize the arrays
			vertexData = new float[127 * 127 * 2 * 3 * 3];
			colorData = new float[127 * 127 * 2 * 3 * 3];
			vertexNormal = new float[127 * 127 * 2 * 3 * 3];
			// keep track of where we are in the array
			int counter = 0;
			// temp vectors for calculating the normals when initializing the arrays
			Vector3 temp1, temp2, temp3, temptot1, temptot2, temptot3;
			temp1 = new Vector3();
			temp2 = new Vector3();
			temp3 = new Vector3();

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
					//normal2
				}

			// reset the counter
			counter = 0;

			// filling the vertexNormal array
			for (int i = 0; i < 127; i++)
				for (int j = 0; j < 127; j++)
				{
					float f = size * 2;
					float di = f * ( i - 63 );
					float dj = f * ( j - 63 );
					temp1.X = -size + di;
					temp1.Y = -size + dj;
					temp1.Z = ( -h[i, j] - depth ) * scale;
					temp2.X = size + di;
					temp2.Y = -size + dj;
					temp2.Z = ( -h[i + 1, j] - depth ) * scale;
					temp3.X = size + di;
					temp3.Y = size + dj;
					temp3.Z = ( -h[i + 1, j + 1] - depth ) * scale;
					temptot1 = Normal(temp1, temp2, temp3);
					vertexNormal[counter++] = temptot1.X;
					vertexNormal[counter++] = temptot1.Y;
					vertexNormal[counter++] = temptot1.Z;
					vertexNormal[counter++] = temptot1.X;
					vertexNormal[counter++] = temptot1.Y;
					vertexNormal[counter++] = temptot1.Z;
					vertexNormal[counter++] = temptot1.X;
					vertexNormal[counter++] = temptot1.Y;
					vertexNormal[counter++] = temptot1.Z;
					temp1.X = -size + di;
					temp1.Y = size + dj;
					temp1.Z = ( -h[i, j + 1] - depth ) * scale;
					temp2.X = size + di;
					temp2.Y = size + dj;
					temp2.Z = ( -h[i + 1, j + 1] - depth ) * scale;
					temp3.X = -size + di;
					temp3.Y = -size + dj;
					temp3.Z = ( -h[i, j] - depth ) * scale;
					temptot1 = Normal(temp1, temp2, temp3);
					vertexNormal[counter++] = temptot1.X;
					vertexNormal[counter++] = temptot1.Y;
					vertexNormal[counter++] = temptot1.Z;
					vertexNormal[counter++] = temptot1.X;
					vertexNormal[counter++] = temptot1.Y;
					vertexNormal[counter++] = temptot1.Z;
					vertexNormal[counter++] = temptot1.X;
					vertexNormal[counter++] = temptot1.Y;
					vertexNormal[counter++] = temptot1.Z;
				}

			// create the program for the shaders
			programID = GL.CreateProgram();
			// load the shaders
			LoadShader("../../shaders/light.glsl",
			 ShaderType.VertexShader, programID, out vsID);
			LoadShader("../../shaders/fs.glsl",
			 ShaderType.FragmentShader, programID, out fsID);
			// link the shader program
			GL.LinkProgram(programID);

			// getting the shader variable id's for later use
			attribute_vpos = GL.GetAttribLocation(programID, "vPosition");
			attribute_vcol = GL.GetAttribLocation(programID, "vColor");
			attribute_vnor = GL.GetAttribLocation(programID, "vNormal");
			uniform_mvpiew = GL.GetUniformLocation(programID, "MVP");
			uniform_mview = GL.GetUniformLocation(programID, "MV");
			uniform_lightpos = GL.GetUniformLocation(programID, "LightPos");

			// link the position array
			vbo_pos = GL.GenBuffer();
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

			// link the color array
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

			// link the normals array
			vbo_nor = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_nor);
			GL.BufferData<float>(BufferTarget.ArrayBuffer,
				(IntPtr) ( vertexNormal.Length * 4 ),
				vertexNormal, BufferUsageHint.StaticDraw
			);
			GL.VertexAttribPointer(attribute_vnor, 3,
				VertexAttribPointerType.Float,
				true, 0, 0
			);

			// tell GL to use the shader program
			GL.UseProgram(programID);
		}

		// returns the normal of a triangle
		// with help from http://stackoverflow.com/questions/1966587/given-3-pts-how-do-i-calculate-the-normal-vector
		public Vector3 Normal(Vector3 a, Vector3 b, Vector3 c)
		{
			Vector3 direction = Vector3.Cross(b - a, c - a);
			Vector3 normalized = Vector3.Normalize(new Vector3(( direction.X ), ( direction.Y ), ( direction.Z )));
			if (normalized.Z > 0)
				return new Vector3(-normalized.X, -normalized.Y, -normalized.Z);
			return normalized;
		}

		// tick: renders one frame
		public override void Tick()
		{
			screen.Clear(0);
			screen.Print("Exercise 10", 2, 2, 0xffffff);
			a = (float) ( ( a + Math.PI / 90 ) % ( 2 * Math.PI ) );
		}

		// renders the GL
		public override void RenderGL()
		{
			// matrix for transorming the world and for drawing
			Matrix4 MVP, MV;
			MVP = Matrix4.CreateFromAxisAngle(new Vector3(0, 0, 1), a);
			MVP *= Matrix4.CreateFromAxisAngle(new Vector3(1, 0, 0), 1.9f);
			MVP *= Matrix4.CreateTranslation(0, 0, -2);
			MV = MVP;
			MVP *= Matrix4.CreatePerspectiveFieldOfView(1.6f, 1.3f, .1f, 1000);
			// light position
			Vector3 lpos = new Vector3(0, 1, 1);

			// set the shader uniforms
			GL.Uniform3(uniform_lightpos, ref lpos);
			GL.UniformMatrix4(uniform_mview, false, ref MV);
			GL.UniformMatrix4(uniform_mvpiew, false, ref MVP);

			// enable the array streams
			GL.EnableVertexAttribArray(attribute_vpos);
			GL.EnableVertexAttribArray(attribute_vcol);
			GL.EnableVertexAttribArray(attribute_vnor);

			// draw the arrays
			GL.DrawArrays(PrimitiveType.Triangles, 0, 127 * 127 * 2 * 3);
		}
	}

} // namespace Template