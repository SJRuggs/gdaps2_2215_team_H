using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LiveWire.Content
{
    /// <summary>
    /// A Machine that the Player can put a Wire into
    /// that alters the open states of various Door Machines
    /// in the level
    /// </summary>
    public class MchnPlugDoorController : Machine
    {
        public MchnPlugDoorController(int x, int y, int width, int height, Texture2D spriteSheet)
            : base(x, y, width, height, spriteSheet)
        {

        }

        /// <summary>
        /// Logic for when the Player interacts with the Door Controller
        /// </summary>
        /// <param name="player">Reference to the Player object initiating the interaction</param>
        public override void PlayerInteract(Player player)
        {
            // TODO: add interactions for each machine (switch statement using enum)
            
        }
    }
}
