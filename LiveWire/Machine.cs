using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LiveWire
{
    /// <summary>
    /// An abstract class for interactable props in the world
    /// </summary>
    /// 

    enum MachineType
    {
        Default
    }

    public class Machine : TileParent
    {
        #region FIELDS --------------------------------------------------------------------------------

            // All necessary fields are inherited from TileParent to reduce redundancy

        #endregion

        #region PROPERTIES -------------------------------------------------------------------------

            // All necessary properties are inherited from TileParent to reduce redundancy

        #endregion

        #region CONSTRUCTORS --------------------------------------------------------------------------------

        public Machine(int x, int y, int width, int height, Texture2D spriteSheet)
        {
            position = new Rectangle(x, y, width, height);
            this.spriteSheet = spriteSheet;
        }

        #endregion

        #region METHODS --------------------------------------------------------------------------------

        public override void Draw(SpriteBatch spriteBatch)
        {
            // TODO: add animations for each machine (switch statement using enum)
        }
        public override void PlayerInteract(Player player)
        {
            // TODO: add interactions for each machine (switch statement using enum)
        }

        #endregion
    }
}
