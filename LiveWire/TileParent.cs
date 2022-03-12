using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiveWire
{
    /// <summary>
    /// abstract class to hold all tiles on the board, so that a single 2D array can reference any tile type.
    /// </summary>
    public abstract class TileParent
    {
        // --- PROPERTIES ---
        public abstract Rectangle Position { get; set; }

        public abstract bool[] AnimState { get; set; }

        public abstract bool BlocksPLayer { get; set; }

        public abstract bool BlocksWire { get; set; }

        public abstract bool InteractsWire { get; set; }

        // --- METHODS ---
        public abstract void Draw(SpriteBatch spriteBatch);

        public abstract void PlayerInteract(Player player);
    }
}
