using System;
using Assignment2Tests;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Pong {
	public interface IPhysicalObject2D {
		float X { get; set; }
		float Y { get; set; }
		int Width { get; set; }
		int Height { get; set; }
	}

	public abstract class Sprite : IPhysicalObject2D
	{
		public float X { get; set; }
		public float Y { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
	}

	public class CollisionDetector {
		public static bool OverlapsWall(IPhysicalObject2D a, IPhysicalObject2D b)
		{
			if (a.X - b.X - b.Width < 5)
			{
				return true;
			}
			return false;
		}
		public static bool OverlapsGoal(IPhysicalObject2D a, IPhysicalObject2D b) {
			if (b.Y - a.Y < 2) {
				return true;
			}
			return false;
		}
		public static bool OverlapsPaddleBottom (IPhysicalObject2D a, IPhysicalObject2D b) {
			if (b.X + b.Width - a.X > 10 && a.X - b.X > -10
					&& b.Y - a.Y < 2 * GameConstants.PaddleDefaulHeight - 5)
			{
				return true;
			}
			return false;
		}
		public static bool OverlapsPaddleTop(IPhysicalObject2D a, IPhysicalObject2D b) {
			if (b.X + b.Width - a.X > 1 && a.X - b.X > -5
					&& a.Y - b.Y < GameConstants.PaddleDefaulHeight - 5) {
				return true;
			}
			return false;
		}
	}

	public class Wall : IPhysicalObject2D {
		public float X { get; set; }
		public float Y { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
		public Wall(float x, float y, int width, int height) {
			X = x;
			Y = y;
			Width = width;
			Height = height;
		}
	}

	public class Background : Game1.Sprite {
		public Background(int width, int height) : base(width, height) {
		}
	}
	public class Ball : Game1.Sprite, IPhysicalObject2D
	{
		public float Speed { get; set; }
		public float BumpSpeedIncreaseFactor { get; set; }
		/// <summary >
		/// Defines ball direction .
		/// Valid values ( -1 , -1) , (1 ,1) , (1 , -1) , ( -1 ,1).
		/// Using Vector2 to simplify game calculation . Potentially
		/// dangerous because vector 2 can swallow other values as well .
		/// </ summary >
		public Vector2 Direction { get; set; }

		public Ball(int size, float speed, float
			defaultBallBumpSpeedIncreaseFactor) : base(size, size) {
			Speed = speed;
			BumpSpeedIncreaseFactor = defaultBallBumpSpeedIncreaseFactor;
			// Initial direction
			Direction = new Vector2(1, 1);
		}
	}
	public class Paddle : Game1.Sprite, IPhysicalObject2D
	{
		/// <summary >
		/// Current paddle speed in time
		/// </ summary >
		public float Speed { get; set; }
		public Paddle(int width, int height, float initialSpeed) : base(width,
			height) {
			Speed = initialSpeed;
		}
		/// <summary >
		/// Overriding draw method . Masking paddle texture with black color .
		/// </ summary >
		public override void DrawSpriteOnScreen(SpriteBatch spriteBatch) {
			spriteBatch.Draw(Texture, new Vector2(X, Y), new Rectangle(0, 0,
				Width, Height), Color.GhostWhite);
		}
	}
	public class GameConstants {
		public const float PaddleDefaulSpeed = 0.9f ;
		public const int PaddleDefaultWidth = 200;
		public const int PaddleDefaulHeight = 20;
		public const float DefaultInitialBallSpeed = 0.4f ;
		public const float DefaultBallBumpSpeedIncreaseFactor = 1.05f ;
		public const int DefaultBallSize = 40;
		public const int WallDefaultSize = 1;
		public const float MaxBallSpeed = 1f;
	}


	public class Game1 : Game {
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		public Game1() {
			graphics = new GraphicsDeviceManager(this) {
				PreferredBackBufferHeight = 900,
				PreferredBackBufferWidth = 500
			};
			Content.RootDirectory = "Content";
		}
		public GenericList<Wall> Walls { get; set; }
		public GenericList<Wall> Goals { get; set; }

		public Paddle PaddleBottom { get; private set; }
		public Paddle PaddleTop { get; private set; }
		public Ball Ball { get; private set; }
		public Background Background { get; private set; }

		public Vector2 FlipX { get; set; }
		public Vector2 FlipY { get; set; }

		//		public SoundEffect HitSound { get; private set; }
		//		public Song Music { get; private set; }


		private IGenericList<Sprite> SpritesForDrawList = new GenericList<Sprite>();


		public abstract class Sprite {
			public float X { get; set; }
			public float Y { get; set; }
			public int Width { get; set; }
			public int Height { get; set; }


			public Texture2D Texture { get; set; }
			protected Sprite(int width, int height, float x = 0, float y = 0) {
				X = x;
				Y = y;
				Height = height;
				Width = width;
			}

			public virtual void DrawSpriteOnScreen(SpriteBatch spriteBatch) {
				spriteBatch.Draw(Texture, new Vector2(X, Y), new Rectangle(0, 0,
					Width, Height), Color.White);
			}
		}







		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		protected override void Initialize() {
			// Screen bounds details . Use this information to set up game objects positions.
			var screenBounds = GraphicsDevice.Viewport.Bounds;
			PaddleBottom = new Paddle(GameConstants.PaddleDefaultWidth, GameConstants.PaddleDefaulHeight,
				GameConstants.PaddleDefaulSpeed);
			PaddleTop = new Paddle(GameConstants.PaddleDefaultWidth, GameConstants.PaddleDefaulHeight, 
				GameConstants.PaddleDefaulSpeed);
			Ball = new Ball(GameConstants.DefaultBallSize, GameConstants.DefaultInitialBallSpeed,
				GameConstants.DefaultBallBumpSpeedIncreaseFactor);

			PaddleBottom.X = screenBounds.Width / 2f - PaddleBottom.Width / 2f;
			PaddleBottom.Y = screenBounds.Bottom - PaddleBottom.Height;
			PaddleTop.X = screenBounds.Width / 2f - PaddleBottom.Width / 2f;
			PaddleTop.Y = screenBounds.Top;// + PaddleBottom.Height;
			Ball = new Ball(GameConstants.DefaultBallSize, GameConstants.DefaultInitialBallSpeed,
				GameConstants.DefaultBallBumpSpeedIncreaseFactor) 
			{
				X = 250,
				Y = 450
			};
			Background = new Background(screenBounds.Width, screenBounds.Height);


			LeftWall = new Wall(-GameConstants.WallDefaultSize - 10, 0,
				GameConstants.WallDefaultSize, screenBounds.Height);
			RightWall = new Wall(screenBounds.Right + 10, 0, GameConstants.WallDefaultSize, screenBounds.Height);
			UpGoal = new Wall(0, screenBounds.Height, screenBounds.Width, GameConstants.WallDefaultSize);
			DownGoal = new Wall(screenBounds.Top, -GameConstants.WallDefaultSize - 20,
				screenBounds.Width, GameConstants.WallDefaultSize);
			// Up/Down--Koo negativ


			SpritesForDrawList.Add(Background);
			SpritesForDrawList.Add(PaddleBottom);
			SpritesForDrawList.Add(PaddleTop);
			SpritesForDrawList.Add(Ball);
			base.Initialize();
		}

		public Wall LeftWall { get; set; }
		public Wall RightWall { get; set; }
		public Wall DownGoal { get; set; }
		public Wall UpGoal { get; set; }

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent() {
			// Initialize new SpriteBatch object which will be used to draw textures .
			spriteBatch = new SpriteBatch(GraphicsDevice);
			// Set textures
			Texture2D paddleTexture = Content.Load<Texture2D>("paddle");
			PaddleBottom.Texture = paddleTexture;
			PaddleTop.Texture = paddleTexture;
			Ball.Texture = Content.Load<Texture2D>("ball");
			Background.Texture = Content.Load<Texture2D>("background");
			// Load sounds
			// Start background music
			HitSound = Content.Load<SoundEffect>("hit");
//			Music = Content.Load<Song>("music");
//			MediaPlayer.IsRepeating = true;
//			MediaPlayer.Play(Music);
		}

		public SoundEffect HitSound { get; set; }

		public Song Music { get; set; }


		protected override void UnloadContent() {
			// TODO: Unload any non ContentManager content here
		}

		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
			    Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();


			var touchState = Keyboard.GetState();
			if (touchState.IsKeyDown(Keys.Left))
			{
				PaddleBottom.X = PaddleBottom.X - (float) (PaddleBottom.Speed *
				                                           gameTime.ElapsedGameTime.TotalMilliseconds);
			}
			if (touchState.IsKeyDown(Keys.Right))
			{
				PaddleBottom.X = PaddleBottom.X + (float) (PaddleBottom.Speed *
				                                           gameTime.ElapsedGameTime.TotalMilliseconds);
			}

			var touchState2 = Keyboard.GetState();
			if (touchState2.IsKeyDown(Keys.A))
			{
				PaddleTop.X = PaddleTop.X - (float) (PaddleTop.Speed *
				                                     gameTime.ElapsedGameTime.TotalMilliseconds);
			}
			if (touchState2.IsKeyDown(Keys.D))
			{
				PaddleTop.X = PaddleTop.X + (float) (PaddleTop.Speed *
				                                     gameTime.ElapsedGameTime.TotalMilliseconds);
			}

			PaddleBottom.X = MathHelper.Clamp(PaddleBottom.X, 0, 500 - PaddleBottom.Width);
			PaddleTop.X = MathHelper.Clamp(PaddleTop.X, 0, 500 - PaddleTop.Width);
			var ballPositionChange = Ball.Direction *
			                         (float) (gameTime.ElapsedGameTime.TotalMilliseconds * Ball.Speed);
			Ball.X += ballPositionChange.X;
			Ball.Y += ballPositionChange.Y;

			if (CollisionDetector.OverlapsWall(Ball, LeftWall) || CollisionDetector.OverlapsWall(RightWall, Ball))
			{
				if (Ball.Speed >= GameConstants.MaxBallSpeed)
				{
					Ball.Speed = GameConstants.MaxBallSpeed;
				}
				else
				{
					Ball.Speed *= Ball.BumpSpeedIncreaseFactor;
				}
				Ball.Direction *= FlipX;
			}
			else if (CollisionDetector.OverlapsPaddleTop(Ball, PaddleTop) || CollisionDetector.OverlapsPaddleBottom(Ball, PaddleBottom))
			{
				HitSound.Play();
				if (Ball.Speed >= GameConstants.MaxBallSpeed) 
				{
					Ball.Speed = GameConstants.MaxBallSpeed;
				}
				else 
				{
					Ball.Speed *= Ball.BumpSpeedIncreaseFactor;
				}
				Ball.Direction *= FlipY;
				if (Ball.Y > 300) Ball.Y -= 10;
				else Ball.Y += 10;
			}
			else if (CollisionDetector.OverlapsGoal(Ball, UpGoal) || CollisionDetector.OverlapsGoal(DownGoal, Ball))
			{
				Ball.X = 250;
				Ball.Y = 450;
				Ball.Speed = GameConstants.DefaultInitialBallSpeed;
			}

			FlipX = new Vector2(-1, 1);
			FlipY = new Vector2(1, -1);

			base.Update(gameTime);
		}


		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime) {
			GraphicsDevice.Clear(Color.CornflowerBlue);

			spriteBatch.Begin();
			for (int i = 0; i < SpritesForDrawList.Count; i++) {
				SpritesForDrawList.GetElement(i).DrawSpriteOnScreen(spriteBatch);
			}

			spriteBatch.End();
			base.Draw(gameTime);
		}
	}
}
