#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Audio;
using SlackHash.Data;
using System.Linq;
#endregion

namespace SlackHash.Game
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
		SoundEffect sound;
		SoundEffectInstance soundInst;
		SpriteFont font;
        Player player1;
        Player player2;
        Dictionary<String, Object> context;

        Texture2D GFX_Laser;

        List<Entity> ents;
        List<Player> players;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

			Window.AllowUserResizing = false;

            context = new Dictionary<String, Object>();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
		{
			graphics.IsFullScreen = true;

            ents = new List<Entity>(100);
            players = new List<Player>(2);

			player1 = new Player( context );
			player1.Scale = 4.0f;
			player1.PlayerIndex = PlayerIndex.One;
            player1.FollowMainMouse = true;
			//player1.ControlBindings.Add("Y_AXIS_NEG", Keys.W);
			//player1.ControlBindings.Add("X_AXIS_NEG", Keys.A);
			//player1.ControlBindings.Add("Y_AXIS_POS", Keys.S);
			//player1.ControlBindings.Add("X_AXIS_POS", Keys.D);
			//player1.ControlBindings.Add("ATTACK", Keys.Space);
            ents.Add(player1);
            players.Add(player1);
            context.Add("player1", player1);

			//if (GamePad.GetState(PlayerIndex.One).IsConnected)
			//{
				player2 = new Player(context);
				player2.Scale = 4.0f;
				player2.PlayerIndex = PlayerIndex.Two;
				//player2.ControlBindings.Add("Y_AXIS_NEG", Keys.Up);
				//player2.ControlBindings.Add("X_AXIS_NEG", Keys.Left);
				//player2.ControlBindings.Add("Y_AXIS_POS", Keys.Down);
				//player2.ControlBindings.Add("X_AXIS_POS", Keys.Right);
				//player2.ControlBindings.Add("ATTACK", GamePad.GetState(PlayerIndex.One).Buttons.A);
			//}
			
            ents.Add(player2);
            players.Add(player2);
            context.Add("player2", player2);

			InputManager.Initialize();
			
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Textures
            spriteBatch = new SpriteBatch(GraphicsDevice);

			Vector2 playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + GraphicsDevice.Viewport.TitleSafeArea.Width / 2,
                                                 GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
			player1.Initialize(Content.Load<Texture2D>("Graphics\\player"), playerPosition, 16, 4);
			playerPosition.X += 40.0f;
			player2.Initialize(Content.Load<Texture2D>("Graphics\\player"), playerPosition, 16, 4);
			player2.Color = Color.Blue;


            GFX_Laser = Content.Load<Texture2D>("Graphics\\laser");
            context.Add("GFX_Laser", GFX_Laser);

			// Font
			font = Content.Load<SpriteFont>("Graphics\\spriteFont1");

			// Sound
			sound = Content.Load<SoundEffect>("Sound\\laserFire");
			soundInst = sound.CreateInstance();
            context.Add("SND_LaserFire", soundInst);


			// Input
			//input = new InputManager();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            foreach (Entity ent in ents)
            {
                ent.Texture.Dispose();
            }

			soundInst.Dispose();
            sound.Dispose();

            graphics.Dispose();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
		{
			float dt = gameTime.ElapsedGameTime.Milliseconds / 1000.0f;

			InputManager.Update();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

			KeyboardState ks = Keyboard.GetState();
            MouseState ms = Mouse.GetState();

            foreach (Player plyr in players)
            {
				if (plyr.IsAlive)
				{
					Vector2 dir = new Vector2(0.0f);

					if (InputManager.IsActionPressed(InputManager.Action.MoveCharacterUp, plyr.PlayerIndex))
						dir.Y = -1.0f;
					else if (InputManager.IsActionPressed(InputManager.Action.MoveCharacterDown, plyr.PlayerIndex))
						dir.Y = 1.0f;
					else
						plyr.Acceleration.Y = 0.0f;

					if (InputManager.IsActionPressed(InputManager.Action.MoveCharacterLeft, plyr.PlayerIndex))
						dir.X = -1.0f;
					else if (InputManager.IsActionPressed(InputManager.Action.MoveCharacterRight, plyr.PlayerIndex))
						dir.X = 1.0f;
					else
						plyr.Acceleration.X = 0.0f;

					// only attempt "move" when needed
					if (dir.Length() > 0.0f)
						plyr.Move((float)Math.Atan2(dir.Y, dir.X));

					// TODO: Figure out a good, scalable way of moving this into the player
					// ATTACK
					if (InputManager.IsActionPressed(InputManager.Action.AttackWeaponOne, plyr.PlayerIndex))
					{
						plyr.Weapon1.Attack();
					}

					// MOUSE
					if (plyr.FollowMainMouse)
					{
						if (plyr.PlayerIndex == PlayerIndex.One)
						{
							var mousePlayerAngle = (float)Math.Atan2(ms.Y - plyr.Position.Y, ms.X - plyr.Position.X);
							plyr.Direction = mousePlayerAngle;
						}
					}
					float thumbStickRotation = (float)Math.Atan2(GamePad.GetState(plyr.PlayerIndex).ThumbSticks.Right.X,
													GamePad.GetState(plyr.PlayerIndex).ThumbSticks.Right.Y);
					if (thumbStickRotation != 0f)
						plyr.Direction = thumbStickRotation - 1.6f;
				}
            }

            foreach( Entity ent in ents )
            {
                ent.Update(gameTime);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // start drawing
            spriteBatch.Begin();
            ////////////////////

            foreach (Player plyr in players)
            {
                if( plyr.IsAlive )
                    plyr.Draw(spriteBatch, gameTime);
            }


			// UI
			spriteBatch.DrawString(font, 
								"Player 1", 
								new Vector2(10, GraphicsDevice.Viewport.TitleSafeArea.Height - 110), 
								Color.Black);
			spriteBatch.DrawString(font,
								"HP: " + player1.Health, 
								new Vector2(10, GraphicsDevice.Viewport.TitleSafeArea.Height - 90),
								Color.Black);
			spriteBatch.DrawString(font,
								"Hat: " + player1.Hat.Name,
								new Vector2(10, GraphicsDevice.Viewport.TitleSafeArea.Height - 70),
								Color.Black);
			spriteBatch.DrawString(font,
								"W1: " + player1.Weapon1.Name,
								new Vector2(10, GraphicsDevice.Viewport.TitleSafeArea.Height - 50),
								Color.Black);
			spriteBatch.DrawString(font,
								"W2: " + player1.Weapon2.Name,
								new Vector2(10, GraphicsDevice.Viewport.TitleSafeArea.Height - 30),
								Color.Black);


			spriteBatch.DrawString(font,
								"Player 2",
								new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Width - 120, 
										GraphicsDevice.Viewport.TitleSafeArea.Height - 110),
								Color.Black);
			spriteBatch.DrawString(font,
								"HP: " + player2.Health,
								new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Width - 120,
										GraphicsDevice.Viewport.TitleSafeArea.Height - 90),
								Color.Black);
			spriteBatch.DrawString(font,
								"Hat: " + player2.Hat.Name,
								new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Width - 120,
										GraphicsDevice.Viewport.TitleSafeArea.Height - 70),
								Color.Black);
			spriteBatch.DrawString(font,
								"W1: " + player2.Weapon1.Name,
								new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Width - 120,
										GraphicsDevice.Viewport.TitleSafeArea.Height - 50),
								Color.Black);
			spriteBatch.DrawString(font,
								"W2: " + player2.Weapon2.Name,
								new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Width - 120,
										GraphicsDevice.Viewport.TitleSafeArea.Height - 30),
								Color.Black);

            ////////////////////
            spriteBatch.End();
            // stop drawing

            base.Draw(gameTime);
        }
    }
}
