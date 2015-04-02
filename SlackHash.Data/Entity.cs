using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SlackHash.Data
{
	public class Entity
	{
		public Texture2D Texture;
		public Vector2 Position;
		public Vector2 Acceleration;
		public Vector2 Velocity;
		public Vector2 Drag;
        public float Scale;
		public float Rotation;
		public float Direction;
		public Color Color;
		public CollisionHull Hull;
        public Entity Parent;
		public Dictionary<String, Object> Context;

		public int FrameWidth;
		public int FrameHeight;
		public int TotalFrames;

		protected float time;
		protected float frameTime;
		protected int frameIndex;

		private Vector2 lastPosition;

		public Entity( Dictionary<String, Object> context_ ) : this( context_, null )
		{}
        public Entity(Dictionary<String, Object> context_, Entity parent_)
        {
            Drag = new Vector2(0.0f);
            Rotation = 0.0f;
            Scale = 1.0f;
            Context = context_;
            Parent = parent_;
			time = 0f;
			frameTime = 0.05f;
			frameIndex = 0;
        }

		public virtual void Update(GameTime gameTime)
		{
			float dt = gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
			Position += Velocity * dt;
			Velocity += Acceleration * dt;
			Velocity *= (Drag * (1 + dt));
		}

		public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
		{
            if (Texture != null)
            {
				if (frameIndex > TotalFrames - 1) frameIndex = 0;

				if (Position != lastPosition)
				{
					time += (float)gameTime.ElapsedGameTime.TotalSeconds;
					while (time > frameTime)
					{
						frameIndex++;
						time = 0f;
					}
				}
				else
				{
					frameIndex = 0;
					time = 0f;
				}

				var source = new Rectangle(frameIndex * FrameWidth,
												   0, FrameWidth, FrameHeight);

				var origin = new Vector2(FrameWidth / 2.0f, FrameHeight / 2.0f);

				var effect = SpriteEffects.None;
				if (Direction < -1.6f || Direction > 1.6f)
					effect = SpriteEffects.FlipHorizontally;

                spriteBatch.Draw(Texture, Position, source, Color,
								Rotation, origin, Scale, effect, 0f);

				lastPosition = Position;
            }
		}

        public void Initialize(Texture2D texture, Vector2 position)
        {
            Texture = texture;
            Position = position;
			Color = Color.White;
			FrameWidth = Texture.Width;
			FrameHeight = Texture.Height;
			TotalFrames = 1;
        }
	}
}
