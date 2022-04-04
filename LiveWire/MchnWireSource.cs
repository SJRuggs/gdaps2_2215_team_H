using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LiveWire
{
    /// <summary>
    /// A Machine that the Player can retrieve a Wire from
    /// </summary>
    public class MchnWireSource : Machine
    {
        #region FIELDS --------------------------------------------------------------------------------

        // The Wire held in this Wire Source Machine
        private Wire wire;

        #endregion

        #region PROPERTIES --------------------------------------------------------------------------------

        public Wire ConnectedWire
        {
            get { return wire; }
        }

        /// <summary>
        /// Returns whether the WireSource has a Wire inside it;
        /// if the Wire is on the map, this returns false.
        /// </summary>
        public bool ContainsWire
        {
            get { return wire == null; }
        }

        #endregion

        #region CONSTRUCTOR --------------------------------------------------------------------------------

        /// <summary>
        /// WireSource constructor calls the base TileParent constructor
        /// </summary>
        /// <param name="x">X position of the machine</param>
        /// <param name="y">Y position of the machine</param>
        /// <param name="width">How large horizontally the machine should be drawn</param>
        /// <param name="height">How large vertically the machine should be drawn</param>
        /// <param name="spriteSheet">The spritesheet used to render the machine</param>
        public MchnWireSource(int x, int y, int width, int height, Texture2D spriteSheet)
            : base(x, y, width, height, spriteSheet)
        {
        }

        #endregion

        public override void Draw(SpriteBatch spriteBatch)
        {
            // TODO: add animations for each machine (switch statement using enum)
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
            // TODO: add interactions for when the Source is in different states

            if (player.HoldingWire != null)
            {
                if (ContainsWire)
                {

                }
            }

        }
    }
}
