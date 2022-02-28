using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LiveWire
{
    /// <summary>
    /// TODO: Add summary
    /// </summary>  
    class Tile
    {
        // --- VARIABLE DELCARATIONS ---
        private Rectangle position;
        private bool blocksPlayer;
        private bool blocksWire;
        private bool[] animState;
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

        private Texture2D spriteSheet;



        // --- PROPERTIES ---
        public Rectangle Position
        {
            get { return position; }
            set { position = value; }
        }

        public bool[] AnimState
        {
            get { return animState; }
            set { animState = value; }
        }

        public bool BlocksPLayer
        {
            get { return blocksPlayer; }
            set { blocksWire = value; }
        }

        public bool BlocksWire
        {
            get { return blocksWire; }
            set { blocksWire = value; }
        }



        // --- CONSTRUCTOR ---
        public Tile(int x, int y, int w, int h, Texture2D spriteSheet, bool isActive)
        {
            position = new Rectangle(x, y, w, h);
            this.spriteSheet = spriteSheet;
            animState = new bool[15];
            animState[0] = isActive;
            animState[14] = !isActive;
        }



        // --- METHODS ---
        public void Draw(SpriteBatch spriteBatch)
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
    }
}
