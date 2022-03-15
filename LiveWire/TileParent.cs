﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiveWire
{
    /// <summary>
    /// Abstract class to hold all tiles on the board, so that a single 2D array can reference any tile type.
    /// </summary>
    public abstract class TileParent
    {
        // --- FIELDS ---
        // The object's location and size on the map
        protected Rectangle position;
        // The sprites used to render the object
        protected Texture2D spriteSheet;
        // Which parts of the sprite should be rendered based on what Tiles border it,
        // used to draw connecting textures
        protected bool[] animState;
        // 0: base anim
        // 1: N border
        // 2: E border
        // 3: S border
        // 4: W border
        // 5: NW inside
        // 6: NE inside
        // 7: SE inside
        // 8: SW inside
        // 9: west inside bridge
        // 10: east inside bridge
        // 11: north inside bridge
        // 12: south inside bridge
        // 13: all bridges
        // 14: empty tile

        // Whether the object collides with the Player
        protected bool blocksPlayer;
        // Whether the object causes Wires to wrap around it
        protected bool blocksWire;
        // Whether the object does anything when the Wire wraps around it
        protected bool interactsWire;

        // --- PROPERTIES ---
        protected Rectangle Position
        {
            get { return position; }
            set { position = value; }
        }

        protected bool[] AnimState
        {
            get { return animState; }
            set { animState = value; }
        }

        protected bool BlocksPLayer
        {
            get { return blocksPlayer; }
            set { blocksWire = value; }
        }

        protected bool BlocksWire
        {
            get { return blocksWire; }
            set { blocksWire = value; }
        }

        protected bool InteractsWire
        {
            get { return interactsWire; }
            set { interactsWire = value; }
        }

        // --- METHODS ---
        public abstract void Draw(SpriteBatch spriteBatch);

        public abstract void PlayerInteract(Player player);
    }
}