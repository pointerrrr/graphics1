﻿using System;
using System.IO;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Windows.Forms;

namespace Template
{
	public class OpenTKApp : GameWindow
	{
		static int screenID;
		static Game game;
		static bool terminated = false;

		public OpenTKApp(int exercise = 0)
		{
			switch (exercise)
			{
				
				case 1:
					game = new Exercise1();
					break;
				case 2:
					game = new Exercise2();
					break;
				case 3:
					game = new Exercise3();
					break;
				case 4:
					game = new Exercise4();
					break;
				case 5:
					game = new Exercise5();
					break;
				case 6:
					game = new Exercise6();
					break;
				case 7:
					game = new Exercise7();
					break;
				case 8:
					game = new Exercise8();
					break;
				default:
					game = new Game();
					break;
			}
		}

		protected override void OnLoad( EventArgs e )
		{
			// called upon app init
			GL.ClearColor( Color.Black );
			GL.Enable( EnableCap.Texture2D );
			GL.Disable( EnableCap.DepthTest );
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
			// convert Game.screen to OpenGL texture
			GL.BindTexture( TextureTarget.Texture2D, screenID );
			GL.TexImage2D( TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, 
						   game.screen.width, game.screen.height, 0, 
						   OpenTK.Graphics.OpenGL.PixelFormat.Bgra, 
						   PixelType.UnsignedByte, game.screen.pixels 
						 );
			// clear window contents
			GL.Clear( ClearBufferMask.ColorBufferBit );
			// setup camera
			GL.MatrixMode( MatrixMode.Modelview );
			GL.LoadIdentity();
			GL.MatrixMode( MatrixMode.Projection );
			GL.LoadIdentity();
			// draw screen filling quad
			GL.Begin( PrimitiveType.Quads );
			GL.TexCoord2( 0.0f, 1.0f ); GL.Vertex2( -1.0f, -1.0f );
			GL.TexCoord2( 1.0f, 1.0f ); GL.Vertex2(  1.0f, -1.0f );
			GL.TexCoord2( 1.0f, 0.0f ); GL.Vertex2(  1.0f,  1.0f );
			GL.TexCoord2( 0.0f, 0.0f ); GL.Vertex2( -1.0f,  1.0f );
			GL.End();
			// tell OpenTK we're done rendering
			SwapBuffers();
		}
		public static void Main( string[] args ) 
		{
			// entry point			
			Console.Write("Enter a number (1-8) to open up the corresponding exercise: ");
			int num;
			if(int.TryParse(Console.ReadLine(), out num))
			{
				if (num < 9 && num > 0)
					using (OpenTKApp app = new OpenTKApp(num)) { app.Run(30.0, 30.0); }
			}
			else
			{
				Console.WriteLine("Please enter a number between 1 and 8.");
			}			
		}
	}
}