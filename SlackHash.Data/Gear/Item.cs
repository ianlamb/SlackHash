using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlackHash.Data
{
    public class Item : Entity
    {

        public Item(Dictionary<String, Object> context_, Entity parent_)
            : base(context_, parent_)
        { }

    }
}
