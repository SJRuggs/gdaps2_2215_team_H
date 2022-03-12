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
        Defualt
    }

    public class Machine : TileParent
    {
        #region FIELDS --------------------------------------------------------------------------------

        // Where on the screen the Machine should be drawn
        private Rectangle position;
        private bool[] animState;
        private bool blocksPlayer;
        private bool blocksWire;
        private bool interactsWire;
        // The sprite(s) to be used for the Machine
        // (each Machine handles sprites differently, so some may have )
        private Texture2D spriteSheet;

        #endregion

        #region Properties -------------------------------------------------------------------------

        public override Rectangle Position
        {
            get { return position; }
            set { position = value; }
        }

        public override bool[] AnimState
        {
            get { return animState; }
            set { animState = value; }
        }

        public override bool BlocksPLayer
        {
            get { return blocksPlayer; }
            set { blocksPlayer = value; }
        }

        public override bool BlocksWire
        {
            get { return blocksWire; }
            set { blocksWire = value; }
        }

        public override bool InteractsWire
        {
            get { return interactsWire; }
            set { interactsWire = value; }
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
