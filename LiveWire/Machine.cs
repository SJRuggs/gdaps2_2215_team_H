﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LiveWire
{
    /// <summary>
    /// An abstract class for a single tile of the level drawn to the screen
    /// that can be interacted with by the Player
    /// </summary>

    public enum MachineType
    {
        WireSource,
        PlugDoorController,
        Door
    }

    public abstract class Machine : TileParent
    {
        #region FIELDS --------------------------------------------------------------------------------

            // Common fields are inherited from TileParent to reduce redundancy

        // What type of Machine the object is
        protected MachineType machineType;

        #endregion

        #region PROPERTIES -------------------------------------------------------------------------

            // All necessary properties are inherited from TileParent to reduce redundancy

        protected MachineType Type
        {
            get { return machineType; }
        }

        #endregion

        #region CONSTRUCTORS --------------------------------------------------------------------------------

        public Machine(int x, int y, int width, int height, Texture2D spriteSheet)
        {
            position = new Rectangle(x, y, width, height);
            this.spriteSheet = spriteSheet;
        }

        #endregion

        #region METHODS --------------------------------------------------------------------------------

        #endregion
    }
}
