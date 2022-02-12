using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace LiveWire
{
    /// <summary>
    /// TODO: Add summary
    /// </summary>
    class Tile
    {
        // --- VARIABLE DELCARATIONS ---
        private Rectangle position;
        private bool isActive;
        private bool[] isBorder;
            // 0: north border
            // 1: east border
            // 2: south border
            // 3: west border



        // --- PROPERTIES ---
        public Rectangle Position
        {
            get { return position; }
            set { position = value; }
        }

        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        public bool[] IsBorder
        {
            get { return IsBorder; }
            set { isBorder = value; }
        }



        // --- CONSTRUCTOR ---
        public Tile(int x, int y, int w, int h, bool isActive, bool[] isBorder)
        {
            position = new Rectangle(x, y, w, h);
            this.isActive = isActive;
            this.isBorder = isBorder;
        }



        // --- METHODS ---

    }
}
