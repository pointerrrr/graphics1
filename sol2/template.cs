﻿using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Template
{
	public class OpenTKApp : GameWindow
	{
		static int screenID;
		static Game game;
		static bool terminated = false;

		public OpenTKApp(int exercise = 0)
		{
			// start the requested exercise
			switch (exercise)
			{
				case 6:
					game = new Exercise6();
					break;
				case 7:
					game = new Exercise7();
					break;
				case 8:
					game = new Exercise8();
					break;
				case 9:
					game = new Exercise9();
					break;
				case 10:
					game = new Exercise10();
					break;
				default:
					game = new Game();
					break;
			}
		}

		protected override void OnLoad( EventArgs e )
		{
			// called upon app init
			GL.Hint( HintTarget.PerspectiveCorrectionHint, HintMode.Nicest );
			ClientSize = new Size( 640, 400);
			if(game==null)
				game = new Game();
			game.screen = new Surface( Width, Height );
			Sprite.target = game.screen;
			screenID = game.screen.GenTexture();
			game.Init();
		}
		protected override void OnUnload( EventArgs e )
		{
			// called upon app close
			GL.DeleteTextures( 1, ref screenID );
			Environment.Exit( 0 ); // bypass wait for key on CTRL-F5
		}
		protected override void OnResize( EventArgs e )
		{
			// called upon window resize
			game.screen = new Surface(Width, Height);
			GL.Viewport(0, 0, Width, Height);
			GL.MatrixMode( MatrixMode.Projection );
			GL.LoadIdentity();
			GL.Ortho( -1.0, 1.0, -1.0, 1.0, 0.0, 4.0 );
		}
		protected override void OnUpdateFrame( FrameEventArgs e )
		{
			// called once per frame; app logic
			var keyboard = OpenTK.Input.Keyboard.GetState();
			if (keyboard[OpenTK.Input.Key.Escape]) this.Exit();

			// tell the game to check keyboard input
			game.Control(keyboard);
		}
		protected override void OnRenderFrame( FrameEventArgs e )
		{
			// called once per frame; render
			game.Tick();
			if (terminated)
			{
				Exit();
				return;
			}
			GL.ClearColor(Color.Black);
			GL.Enable(EnableCap.Texture2D);
			GL.Disable(EnableCap.DepthTest);
			GL.Color3(1.0f, 1.0f, 1.0f);
			// convert Game.screen to OpenGL texture
			GL.BindTexture(TextureTarget.Texture2D, screenID);
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
						   game.screen.width, game.screen.height, 0,
						   OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
						   PixelType.UnsignedByte, game.screen.pixels
						 );
			// clear window contents
			GL.Clear(ClearBufferMask.ColorBufferBit);
			// setup camera
			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadIdentity();
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			// draw screen filling quad
			GL.UseProgram(0);
			GL.Begin(PrimitiveType.Quads);
			GL.TexCoord2(0.0f, 1.0f); GL.Vertex2(-1.0f, -1.0f);
			GL.TexCoord2(1.0f, 1.0f); GL.Vertex2(1.0f, -1.0f);
			GL.TexCoord2(1.0f, 0.0f); GL.Vertex2(1.0f, 1.0f);
			GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(-1.0f, 1.0f);
			GL.End();
			GL.UseProgram(game.programID);
			// prepare for generic OpenGL rendering
			GL.Enable(EnableCap.DepthTest);
			GL.Disable(EnableCap.Texture2D);
			GL.Clear(ClearBufferMask.DepthBufferBit);			// tell the game to render GL
			game.RenderGL();
			// tell OpenTK we're done rendering
			SwapBuffers();
		}
		public static void Main( string[] args ) 
		{
			// a prompt asking which exercise to start		
			Console.Write("Enter a number (6-10) to open up the corresponding exercise: ");
			int num;
			if(int.TryParse(Console.ReadLine(), out num))
			{
				if (num < 11 && num > 5)
					using (OpenTKApp app = new OpenTKApp(num)) { app.Run(30.0, 30.0); }
			}
			else
			{
				Console.WriteLine("Please enter a number between 6 and 10.");
			}			
		}
	}
}