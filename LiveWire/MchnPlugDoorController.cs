using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LiveWire
{
    /// <summary>
    /// A Machine that the Player can plug a Wire into
    /// and opens or closes a list of other DoorSegment Machines
    /// in the level
    /// </summary>
    public class MchnPlugDoorController : Machine
    {
        #region CONSTANTS --------------------------------------------------------------------------------

        // DoorController can be tinted to differentiate from normal tiles
        private Color doorControllerTint = Color.Aqua;

        #endregion

        #region FIELDS --------------------------------------------------------------------------------

        ///<summary>Stores all Door tiles this machine controls the state of</summary>
        private List<MchnDoorSegment> doorTiles;
        ///<summary>The Wire plugged into this Door Controller Machine</summary>
        private Wire wire;

        

        #endregion

        #region PROPERTIES -------------------------------------------------------------------------

        /// <summary>
        /// The Wire connected to the Door Controller;
        /// returns null if none is connected
        /// </summary>
        public Wire ConnectedWire
        {
            get { return wire; }
        }

        public Color DoorColor
        {
            get; set;
        }
        #endregion

        #region CONSTRUCTORS --------------------------------------------------------------------------------

        /// <summary>
        /// DoorController constructor calls the base TileParent constructor
        /// </summary>
        /// <param name="x">X position of the machine on the screen</param>
        /// <param name="y">Y position of the machine on the screen</param>
        /// <param name="width">How large horizontally the machine should be drawn</param>
        /// <param name="height">How large vertically the machine should be drawn</param>
        /// <param name="spriteSheet">The spritesheet used to render the machine</param>
        /// <param name="doors">The list of MchnDoorSegments this machine controls</param>
        public MchnPlugDoorController
            (int x, int y, int width, int height, Texture2D spriteSheet, List<MchnDoorSegment> doorTiles, int color)
            : base(x, y, width, height, spriteSheet)
        {
            blocksPlayer = false;
            blocksWire = false;
            interactsWire = false;
            this.doorTiles = doorTiles;
            switch (color)
            {
                case 1:
                    this.DoorColor = Color.Red;
                    break;
                case 2:
                    this.DoorColor = Color.Green;
                    break;
                case 3:
                    this.DoorColor = Color.Blue;
                    break;
                case 4:
                    this.DoorColor = Color.Yellow;
                    break;
                default:
                    this.DoorColor = Color.White;
                    break;

            }
            
            IsActive = true;
        }

        #endregion

        #region METHODS --------------------------------------------------------------------------------
        /// <summary>
        /// Logic for when the Player tries to interact with the DoorController
        /// </summary>
        /// <param name="player">Reference to the Player object initiating the interaction</param>
        public override void PlayerInteract(Player player)
        {
            System.Diagnostics.Debug.WriteLine("I MchnPlugDoorController was interacted with");

            /*

            // Unused code scaffolding for checking if the player has a wire first,
            // written back when we had planned to have the wires be retrieved from
            // various stations instead of being with you at all times in the level

            // If the Player is holding a Wire,
            if (player.HoldingWire != null)
            {
                // Connect it to the Machine; if this replaced another Wire,
                // the old Wire is given to the Player
                player.HoldingWire = ConnectWire(player.HoldingWire);
            }
            // If the Player is not holding a Wire but there's a Wire in the Machine,
            else if (wire != null)
            {
                // Disconnect this Wire and give it to the Player
                player.HoldingWire = DisconnectWire();
            }
            // If nobody has a Wire, do nothing

            */

            // If the Player is within about two player widths of the door controller,
            // toggle the doors the machine is connected to
            if (Vector2.Distance(player.Center(), Center) < (player.Dimensions.X * 2))
            {
                ToggleDoors();
            }
        }

        /// <summary>
        /// Connects a new Wire and toggles all Doors,
        /// disconnecting an old Wire if there is one
        /// Unused in current build
        /// </summary>
        /// <param name="newWire">The Wire to be connected to the DoorController</param>
        /// <returns>The Wire that used to be connected to the DoorController,
        /// or null if there was none</returns>
        private Wire ConnectWire(Wire newWire)
        {
            // Disconnects an existing Wire if there was one
            Wire oldWire = DisconnectWire();
            // If the new Wire was actually connected,
            if (wire != null)
            {
                // (If an old Wire was replaced and DisconnectWire toggled once,
                // this would serve to switch it back to its original state)
                ToggleDoors();
                // Sets the Machine's stored wire to the new Wire
                wire = newWire;
                // TODO: Give the newWire a reference to this Machine
            }
            // Returns the old Wire if one was disconnected, or null
            return oldWire;
        }

        /// <summary>
        /// Disconnects the Wire currently plugged in and toggles all Doors
        /// Unused in current build
        /// </summary>
        /// <returns>The old Wire that was just disconnected</returns>
        private Wire DisconnectWire()
        {
            // Keep a reference to the old wire to be returned
            Wire oldWire = wire;
            // If a Wire was actually disconnected,
            if (oldWire != null)
            {
                ToggleDoors();
                // TODO: Dereference this Machine from the oldWire
                // Dereference the old Wire
                wire = null;
            }
            // Returns the now-disconnected Wire
            return oldWire;
        }

        /// <summary>
        /// Calls the Toggle() method on all doors attached to this DoorController
        /// </summary>
        private void ToggleDoors()
        {
            foreach (MchnDoorSegment door in doorTiles)
            {
                door.Toggle();
            }
        }

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
                         DoorColor);
                }
            }
        }

        #endregion
    }
}
