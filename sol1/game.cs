using System;
using System.IO;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;

namespace Template {
	
	// base class for all exercises
	class Game
	{
		// member variables
		public Surface screen;

		protected  KeyboardState prevKeyState, currentKeyState;

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

		// for rendering GL
		public virtual void RenderGL()
		{

		}

		// update what keys are pressed and act for that
		public virtual void Control(KeyboardState keys)
		{
			prevKeyState = keys;
		}

		// creates an integer color from RGB
		public static int CreateRGB(int red, int green, int blue)
		{
			return ( red << 16 ) + ( green << 8 ) + blue;
		}

		// checks if a key was pressed this frame, or if it was already pressed
		public bool NewKeyPress(Key key)
		{
			return ( currentKeyState[key] && ( currentKeyState[key] != prevKeyState[key] ) );
		}
	}
} // namespace Template