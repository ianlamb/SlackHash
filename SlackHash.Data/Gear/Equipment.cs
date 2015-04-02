using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SlackHash.Data
{
	public class Equipment : Item
    {

        public Equipment(Dictionary<String, Object> context_, Entity parent_)
            : base(context_, parent_)
        { }

    }
}
