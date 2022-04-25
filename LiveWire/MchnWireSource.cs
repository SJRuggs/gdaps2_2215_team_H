using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LiveWire
{
    /// <summary>
    /// A Machine that the Player can retrieve a Wire from;
    /// unused in this version of the game
    /// </summary>
    public class MchnWireSource : Machine
    {
        #region CONSTANTS --------------------------------------------------------------------------------

        // WireSource can be tinted to differentiate from normal tiles
        private Color wireSourceTint = Color.Yellow;

        #endregion

        #region FIELDS --------------------------------------------------------------------------------

        // The Wire held in this Wire Source Machine
        private Wire wire;
        // Whether the Wire has been taken out of the WireSource,
        // and is either in the Player's hand or connected to another Machine
        private bool isWireDrawn;

        #endregion

        #region PROPERTIES --------------------------------------------------------------------------------

        /// <summary>
        /// The Wire stored in the WireSource
        /// </summary>
        public Wire ConnectedWire
        {
            get { return wire; }
        }

        /// <summary>
        /// Returns whether the WireSource has a Wire inside it;
        /// if the Wire is on the map, this returns true.
        /// </summary>
        public bool IsWireDrawn
        {
            get { return isWireDrawn; }
        }

        #endregion

        #region CONSTRUCTOR --------------------------------------------------------------------------------

        /// <summary>
        /// WireSource constructor calls the base TileParent constructor
        /// </summary>
        /// <param name="x">X position of the machine on the screen</param>
        /// <param name="y">Y position of the machine on the screen</param>
        /// <param name="width">How large horizontally the machine should be drawn</param>
        /// <param name="height">How large vertically the machine should be drawn</param>
        /// <param name="spriteSheet">The spritesheet used to render the machine</param>
        public MchnWireSource(int x, int y, int width, int height, Texture2D spriteSheet)
            : base(x, y, width, height, spriteSheet)
        {
            // TODO: Initialize this created Wire with proper information to make one
            // endpoint follow the Player and one originate from the WireSource;
            // it won't be drawn on screen when first initialized but it should exist
            // TODO: Pass the Wire a reference to the WireSource
            // wire = new Wire();

            blocksPlayer = true;
            blocksWire = false;
            interactsWire = false;
            IsActive = true;
        }

        #endregion

        #region METHODS --------------------------------------------------------------------------------


        // Scrapped code for rendering Machines with a tint
        /*
        /// <summary>
        /// Draws the Wire Source to the screen
        /// </summary>
        /// <param name="spriteBatch">The currently open sprite batch</param>
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
                        wireSourceTint);
                }
            }
        }
        */

        /// <summary>
        /// Logic for when the Player tries to interact with the WireSource
        /// </summary>
        /// <param name="player">Reference to the Player object initiating the interaction</param>
        public override void PlayerInteract(Player player)
        {
            System.Diagnostics.Debug.WriteLine("I MchnWireSource was interacted with");
            // If the Wire is currently on the map, remove it from the map
            if (IsWireDrawn)
            {
                // Stop it from being drawn, and properly remove references to the
                // Player or Machine its endpoint is at
                isWireDrawn = false;

                // TODO: Remove the Wire's reference to the Machine or Player
                // TODO: If the Wire is plugged into a Machine,
                // get a reference to the Machine and call its DisconnectWire() method
                // TODO: Otherwise, if the Wire is held by the Player,
                // remove the Player's reference to the Wire
            }
            // If the Wire is not on the map, put it on the map and give it to the Player
            else
            {
                // If the Player already has a Wire, replace it with this one
                if (player.HoldingWire != null)
                {
                    // TODO: Find the WireSource the Player's Wire is originating from
                    // and set its isWireDrawn to false
                    // TODO: Remove that Wire's reference to the Player holding it
                    // TODO: Remove the Player's reference to that old Wire
                }

                // Give the Player the Wire from this machine to hold
                // TODO: Just wanted to make sure, is this the
                // correct way to give a Wire to the Player?
                player.HoldingWire = wire;
            }
        }

        #endregion
    }
}
