using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SlackHash.Data
{
	public class Hat : Equipment
    {
		public string Name;

        public Hat(Dictionary<String, Object> context_, Entity parent_)
            : base(context_, parent_)
		{
			Name = "None";
		}

    }
}
