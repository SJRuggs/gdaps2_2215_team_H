using Microsoft.Xna.Framework;
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
        // Whether the object does anything when the Wire creates a node around it
        protected bool interactsWire;
        // Whether the object is a spike

        // --- PROPERTIES ---
        public bool IsActive { get; set; }
        public bool IsSpike { get; set; }
        public bool IsFlag { get; set; }
        public Rectangle Position
        {
            get { return position; }
            set { position = value; }
        }

        public Vector2 Center
        {
            get { return position.Center.ToVector2(); }
        }

        public bool[] AnimState
        {
            get { return animState; }
            set { animState = value; }
        }

        public bool BlocksPLayer
        {
            get { return blocksPlayer; }
            set { blocksPlayer = value; }
        }

        public bool BlocksWire
        {
            get { return blocksWire; }
            set { blocksWire = value; }
        }

        public bool InteractsWire
        {
            get { return interactsWire; }
            set { interactsWire = value; }
        }

        // --- METHODS ---

        // Default Draw method, which may be overridden in child classes
        // if they need to support animated textures
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < animState.Length; i++)
            {
                if (animState[i])
                {
                    spriteBatch.Draw(
                        spriteSheet,
                        new Vector2(
                            position.X,
                            position.Y),
                        new Rectangle(
                            i * position.Width,
                            0,
                            40,
                            40),
                        Color.White);
                }
            }
        }

        public abstract void PlayerInteract(Player player);
    }
}
