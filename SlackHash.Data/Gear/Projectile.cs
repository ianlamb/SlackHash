using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SlackHash.Data
{
    class Projectile : Entity
    {
        public float MoveSpeed;
		public float Damage;

        public Projectile(Dictionary<String, Object> context_, Entity parent_) : base(context_, parent_)
        {
            MoveSpeed = 800f;
			Damage = 0.3f;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            float dt = gameTime.ElapsedGameTime.Milliseconds / 1000.0f;

            Velocity.X = (float)Math.Cos(Rotation) * MoveSpeed;
            Velocity.Y = (float)Math.Sin(Rotation) * MoveSpeed;

			List<Player> otherPlayers = Context.Values.Where(x => x is Player && x != this.Parent.Parent).Cast<Player>().ToList();
			if (otherPlayers != null)
			{
				foreach (Player p in otherPlayers)
				{
					Vector2 dist = new Vector2();
					dist.X = p.Position.X - this.Position.X;
					dist.Y = p.Position.Y - this.Position.Y;
					if (dist.Length() < 100 && p.IsAlive)
					{
						p.Health -= this.Damage;
						p.IsAlive = p.Health > 0;
					}
				}
			}

            base.Update(gameTime);
        }
    }
}
