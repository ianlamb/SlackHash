using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SlackHash.Data
{
    public class RangedWeapon : Weapon
    {

        private List<Projectile> _projectiles;
        private int _nextAttackMark;
        private int _gtTotalMs;

        public RangedWeapon(Dictionary<String, Object> context_, Entity parent_) : base(context_, parent_)
        {
            _projectiles = new List<Projectile>(50000);
            RepeatDelay = 150;
            _gtTotalMs = 0;
        }

        public override void Attack()
        {
            if (_nextAttackMark <= _gtTotalMs)
            {
                var projectile = new Projectile(Context, this);
                projectile.Initialize(Context["GFX_Laser"] as Texture2D, this.Parent.Position);
                projectile.Rotation = Parent.Direction;
                _projectiles.Add(projectile);

                SoundEffectInstance snd = (SoundEffectInstance)Context["SND_LaserFire"];
                snd.Stop();
                snd.Play();

                _nextAttackMark = _gtTotalMs + RepeatDelay;

                base.Attack();
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            _gtTotalMs = (int)gameTime.TotalGameTime.TotalMilliseconds;

			foreach (Projectile proj in _projectiles)
			{
				proj.Update(gameTime);
			}

			base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
			foreach (Projectile proj in _projectiles)
			{
				proj.Draw(spriteBatch, gameTime);
			}

            base.Draw(spriteBatch, gameTime);
        }

    }
}
