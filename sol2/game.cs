using System;
using System.IO;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;

namespace Template {

	// base class for all exercises
	class Game
	{
		// member variables
		protected KeyboardState prevKeyState, currentKeyState;
		public Surface screen;		
		public int programID;		

		// initialize
		public virtual void Init()
		{
		}

		// tick: renders one frame
		public virtual void Tick()
		{
			screen.Clear( 0 );
			screen.Print( "hello world", 2, 2, 0xffffff );
			screen.Line(2, 20, 160, 20, 0xff0000);
		}

		// method for rendering the GL
		public virtual void RenderGL()
		{

		}

		// method for handling inputs
		public virtual void Control(KeyboardState keys)
		{
			prevKeyState = keys;
		}

		// create a color int from RGB
		public static int CreateRGB(int red, int green, int blue)
		{
			return ( red << 16 ) + ( green << 8 ) + blue;
		}

		// check if a key was pressed this frame
		public bool NewKeyPress(Key key)
		{
			return ( currentKeyState[key] && ( currentKeyState[key] != prevKeyState[key] ) );
		}


		// load a shader
		protected void LoadShader(String name, ShaderType type, int program, out int ID)
		{
			ID = GL.CreateShader(type);
			using (StreamReader sr = new StreamReader(name))
				GL.ShaderSource(ID, sr.ReadToEnd());
			GL.CompileShader(ID);
			GL.AttachShader(program, ID);
			Console.WriteLine(GL.GetShaderInfoLog(ID));
		}
	}
} // namespace Template