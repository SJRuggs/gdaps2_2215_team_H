﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LiveWire
{
    /// <summary>
    /// TODO: Add summary
    /// </summary>  
    public class Tile : TileParent
    {
        // --- VARIABLE DELCARATIONS ---
        
            // All necessary fields are inherited from TileParent to reduce redundancy

        // --- PROPERTIES ---

            // All necessary properties are inherited from TileParent to reduce redundancy

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
        public override void Draw(SpriteBatch spriteBatch)
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

        public override void PlayerInteract(Player player)
        {
            // do nothing
        }
    }
}
