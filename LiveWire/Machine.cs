using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LiveWire
{
    /// <summary>
    /// An abstract class for interactable props in the world
    /// </summary>
    public abstract class Machine
    {
        #region FIELDS --------------------------------------------------------------------------------

        // Where on the screen the Machine should be drawn
        private Rectangle position;
        // The sprite(s) to be used for the Machine
        // (each Machine handles sprites differently, so some may have )
        private Texture2D spriteSheet;

        #endregion

        #region CONSTRUCTORS --------------------------------------------------------------------------------

        public Machine(int x, int y, int width, int height, Texture2D spriteSheet)
        {
            position = new Rectangle(x, y, width, height);
            this.spriteSheet = spriteSheet;
        }

        #endregion

        #region ABSTRACT METHODS --------------------------------------------------------------------------------

        /// <summary>
        /// Draws the Machine to the screen
        /// </summary>
        /// <param name="spriteBatch">The current Sprite Batch</param>
        public abstract void Draw(SpriteBatch spriteBatch);

        /// <summary>
        /// Functionality for when the Player interacts with the Machine
        /// </summary>
        /// <param name="playerPos">The Player's position Rectangle</param>
        /// <param name="playerHasWire">Whether or not the Player is currently holding a Wire</param>
        public abstract void PlayerInteract(Rectangle playerPos, bool playerHasWire);

        #endregion
    }
}
