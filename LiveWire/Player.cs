using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LiveWire
{

    /// <summary>
    /// TODO: Add Summary
    /// </summary>

    // --- FINATE STATE MACHINE : ANIMSTATE
    enum AnimState
    {
        StandingRight,
        StandingLeft,
        WalkingRight,
        WalkingLeft,
        FallingRight,
        FallingLeft
    }

    class Player
    {
        // --- VARIABLE DELCARATIONS ---
        // animation variables
        private AnimState currentState;

        // physics variables
        private Vector2 position;
        private Vector2 prevPosition;
        private Vector2 velocity;
        private const float speed = 5; // how fast you move in the x axis
        private const float jumpForce = 5; // how strong your jump is
        private const float gravity = 1; // how strong gravity is
        private Rectangle box;
        private Vector2 dimensions;

        // interaction variables
        private bool isHoldingWire;
        private Wire holdingWire;

        // user input keys
        private KeyboardState kbState;
        private KeyboardState prevKbState;
        private Keys left;
        private Keys right;
        private Keys jump;
        private Keys interact;



        // --- PROPERTIES ---
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public AnimState CurrentState
        {
            get { return currentState; }
            set { currentState = value; }
        }

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public Wire HoldingWire
        {
            get { return holdingWire; }
            set { holdingWire = value; }
        }



        // --- CONSTRUCTOR ---
        public Player(Vector2 position, Texture2D playerSprite)
        {
            this.position = position;
            this.dimensions = new Vector2(playerSprite.Width,playerSprite.Height);
            box = new Rectangle(position.ToPoint(), dimensions.ToPoint());
            prevPosition = position;
        }
        // --- METHODS ---

        // updates the Player animation by its animstate
        public void UpdateFSM()
        {
            switch (currentState)
            {
                // TODO: Add cases
            }
        }

        /// <summary>
        /// player <-> enviroment
        /// Takes in input from user (left,right,jump) and moves the player accordingly to physics (gravity, colliding with ground, ground friction) 
        /// </summary>
        public void PlayerMovement() {
            //horizontal velocity
            //reset horizontal velocity
            velocity.X = 0;
            //change horizontal velocity with left and right inputs
            if (kbState.IsKeyDown(left)){ velocity.X -= 1; }
            if (kbState.IsKeyDown(right)){ velocity.X += 1; }
            //change horizontal velocity with speed
            velocity.X *= speed;

            //vertical velocity
            //update vertical velocity with jump force
            if (kbState.IsKeyDown(jump) && prevKbState.IsKeyUp(jump)){ velocity.Y -= jumpForce; }
            //update vertical velocity with gravity
            velocity.Y += gravity;

            //position/collision
            //update position with velocity
            position += velocity;
            //if position colliding with block, adjust position to not be in block and adjust velocity to not move you in to block
            //is collision reactive or proactive?
            //reactive

            //saving last frame position of player
            prevPosition = position;
        }
        

        /// <summary>
        /// player <-> wire
        /// Takes in input from user (interact) Grabs the end of a wire if close enough, releases the wire, or connectes it to a power node if close enough
        /// moves the end of the wire with player if holding wire
        /// </summary>
        public void InteractWire() {
            //holding wire
        //move wire with player
        //if user presses interact and near a power node then place the wire on power node
        //else drop the wire

            //not holding wire
        //if near the end of wire pick it up
        }
    }
}
