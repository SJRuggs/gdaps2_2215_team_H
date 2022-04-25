using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LiveWire
{
    /// <summary>
    /// 
    /// </summary>
    public class MchnDoorSegment : Machine
    {
        #region CONSTANTS --------------------------------------------------------------------------------

        // WireSource can be tinted to differentiate from normal tiles
        /// <summary>The color to tint Doors when they're open</summary>
        private Color openTint = Color.Black;
        /// <summary>The color to tint Doors when they're closed</summary>
        private Color closedTint = Color.Gainsboro;

        #endregion

        #region PROPERTIES --------------------------------------------------------------------------------

        /// <summary>
        /// Whether the Door is open and can be passed through by the Player;
        /// false is closed, true is open
        /// </summary>
        public bool IsOpen
        {
            get { return !IsActive; }
        }

        public float Rotation
        {
            get; set;
        }

        public Color DoorColor
        {
            get; set;
        }

        #endregion

        #region CONSTRUCTOR --------------------------------------------------------------------------------

        /// <summary>
        /// DoorSegment constructor calls the base TileParent constructor
        /// </summary>
        /// <param name="x">X position of the machine on the screen</param>
        /// <param name="y">Y position of the machine on the screen</param>
        /// <param name="width">How large horizontally the machine should be drawn</param>
        /// <param name="height">How large vertically the machine should be drawn</param>
        /// <param name="spriteSheet">The spritesheet used to render the machine</param>
        /// <param name="isOpen">Whether this door is open by default; false is closed, true is open</param>
        public MchnDoorSegment(int x, int y, int width, int height, Texture2D spriteSheet, bool isOpen, int rotation)
            : base(x, y, width, height, spriteSheet)
        {
            blocksWire = false;
            interactsWire = false;
            // Whether this Machine blocks the Player is determined by the constructor
            IsActive = !isOpen;
            // Whether this Machine is Horizontal or not
            if(rotation == 0)
            {
                Rotation = 0;
            }
            else
            {
                Rotation = MathF.PI / 2;
            }
            blocksPlayer = IsActive;
        }

        #endregion

        #region METHODS --------------------------------------------------------------------------------

        /// <summary>
        /// Toggles whether the DoorSegment is open or closed
        /// </summary>
        public void Toggle()
        {
            IsActive = !IsActive;
            blocksPlayer = IsActive;
            animState[19] = !IsOpen;
            animState[20] = IsOpen;

            // check for wire snap
            if (!IsOpen)
            {
                if (Game1.wire.Update(Game1.board))
                {
                    Game1.NewLevel(Game1.currentLevel);
                }
            }
        }

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
                    if (IsOpen)
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
                            openTint);
                    }
                    else
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
                            closedTint);
                    }
                }
            }
        }
        */

        /// <summary>
        /// DoorSegments cannot be interacted with; this method does nothing
        /// </summary>
        /// <param name="player">Reference to the Player object initiating the interaction</param>
        public override void PlayerInteract(Player player)
        {
            System.Diagnostics.Debug.WriteLine("I MchnDoorSegement was interacted with");

            // Do nothing
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < animState.Length; i++)
            {
                if (animState[i])
                {
                    if (Rotation != 0)
                    {
                        spriteBatch.Draw(
                            spriteSheet,
                            new Vector2(
                                position.X + 40,
                                position.Y),
                            new Rectangle(
                                i * position.Width,
                                0,
                                40,
                                40),
                            DoorColor, Rotation, Vector2.Zero, 1, SpriteEffects.None, 1); ;
                    }
                    else
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
                            DoorColor, 0, Vector2.Zero, 1, SpriteEffects.None, 1); ;
                    }
                }
            }
        }
        #endregion
    }
}
