using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SlackHash.Data
{
	public class Weapon : Equipment
    {
		public string Name;
		public float Damage;
		public float Speed;
        public int RepeatDelay; /// delay between attacks in milliseconds

		public Weapon(Dictionary<String,Object> context_, Entity parent_) : base(context_, parent_)
		{
			Name = "None";
			Damage = 1.0f;
			Speed = 1.0f;
            RepeatDelay = 0;
		}

		public virtual void Attack()
		{
		}
    }
}
