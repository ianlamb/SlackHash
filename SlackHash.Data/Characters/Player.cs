using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace SlackHash.Data
{
    public class Player : Entity
    {
		public Vector2 MaxVelocity;
		public Vector2 MoveAcceleration;
        public bool Active;
		public float Health;
		public Hat Hat;
		public Weapon Weapon1;
		public Weapon Weapon2;
		public bool IsAlive;
        public Dictionary<String, Keys> ControlBindings;
        public bool FollowMainMouse;
		public PlayerIndex PlayerIndex;

        private int _nextAttackMark;

        public Player( Dictionary<String, Object> context_ ) : base( context_ )
        {
            ControlBindings = new Dictionary<string, Keys>();
            Active = true;
            Health = 100.0f;
            Hat = new Hat(context_, this);
            Weapon1 = new RangedWeapon(context_, this);
            Weapon2 = new RangedWeapon(context_, this);
            IsAlive = true;
            MaxVelocity = new Vector2(200.0f);
            MoveAcceleration = new Vector2(50.0f);
            Drag = new Vector2(0.9f);
            FollowMainMouse = false;
        }

        public void SampleInput(KeyboardState ks, MouseState ms, GameTime gt)
        {
            // KEYBOARD
            Vector2 dir = new Vector2(0.0f);

            if (ks.IsKeyDown(ControlBindings["Y_AXIS_NEG"]))
                dir.Y = -1.0f;
            else if (ks.IsKeyDown(ControlBindings["Y_AXIS_POS"]))
                dir.Y = 1.0f;
            else
                this.Acceleration.Y = 0.0f;

            if (ks.IsKeyDown(ControlBindings["X_AXIS_NEG"]))
                dir.X = -1.0f;
            else if (ks.IsKeyDown(ControlBindings["X_AXIS_POS"]))
                dir.X = 1.0f;
            else
                this.Acceleration.X = 0.0f;

            // only attempt "move" when needed
            if (dir.Length() > 0.0f)
                this.Move((float)Math.Atan2(dir.Y, dir.X));

            // TODO: Figure out a good, scalable way of moving this into the player
            // ATTACK
            if ( ControlBindings.ContainsKey("ATTACK") && ks.IsKeyDown(ControlBindings["ATTACK"]) )
            {
                Weapon1.Attack();

                /*
                Player otherPlayer = Context.First(x => x.Value is Player && x.Value != this).Value as Player;
                if (otherPlayer != null)
                {
                    Vector2 dist = new Vector2();
                    dist.X = otherPlayer.Position.X - this.Position.X;
                    dist.Y = otherPlayer.Position.Y - this.Position.Y;
                    if (dist.Length() < 100 && otherPlayer.IsAlive)
                    {
                        Weapon1.Attack();
                        otherPlayer.Health -= this.Weapon1.Damage;
                        otherPlayer.IsAlive = otherPlayer.Health > 0;
                    }
                }
                */
            }

            // MOUSE
            if (FollowMainMouse)
            {
                this.Rotation = (float)Math.Atan2(ms.Y - this.Position.Y,
                                                  ms.X - this.Position.X);
            }
        }

		public override void Update(GameTime gameTime)
		{
			float dt = gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
			Position += Velocity * dt;
			Velocity += Acceleration * dt;
			Velocity *= (Drag * (1 + dt));

			// DEATH
			if (Health <= 0f)
			{
				IsAlive = false;
				Health = 0f;
			}

            Weapon1.Update(gameTime);
            Weapon2.Update(gameTime);
		}

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Weapon1.Draw(spriteBatch, gameTime);
            Weapon2.Draw(spriteBatch, gameTime);
            base.Draw(spriteBatch, gameTime);
        }

		public void Initialize(Texture2D texture, Vector2 position, int? frameWidth = null, int? totalFrames = null)
		{
			base.Initialize(texture, position);

			if (frameWidth != null)
			{
				FrameWidth = (int)frameWidth;
			}
			if (totalFrames != null)
			{
				TotalFrames = (int)totalFrames;
			}
		}

		public void Move(float angleRads)
		{
			Velocity.X += MoveAcceleration.X * (float)Math.Cos(angleRads);
			Velocity.Y += MoveAcceleration.Y * (float)Math.Sin(angleRads);
			if (Velocity.Length() >= MaxVelocity.Length())
			{
				Velocity.Normalize();
				Velocity *= MaxVelocity;
			}
		}
    }
}
