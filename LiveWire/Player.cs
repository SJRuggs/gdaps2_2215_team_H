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
    public enum AnimState
    {
        StandingRight,
        StandingLeft,
        WalkingRight,
        WalkingLeft,
        FallingRight,
        FallingLeft
    }

    public class Player
    {
        // --- VARIABLE DELCARATIONS ---
        // animation variables
        private AnimState currentState;

        // physics variables
        private Vector2 position;
        private Vector2 prevPosition;
        private Vector2 velocity;
        private float speed; // how fast you move in the x axis
        private float jumpForce; // how strong your jump is
        private float gravity; // how strong gravity is
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

        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public float JumpForce
        {
            get { return jumpForce; }
            set { jumpForce = value; }
        }

        public float Gravity
        {
            get { return gravity; }
            set { gravity = value; }
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
            left = Keys.Left;
            right = Keys.Right;
            jump = Keys.Up;
            interact = Keys.Space;
            speed = 5;
            jumpForce = 5;
            gravity = 1;
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
        public void PlayerMovement(KeyboardState kbState, KeyboardState prevKbState)
        {
            //horizontal velocity
            //change horizontal velocity with left and right inputs
            if (kbState.IsKeyDown(left)){ velocity.X = -1; }
            else if (kbState.IsKeyDown(right)){ velocity.X = 1; }
            //reset horizontal velocity
            else { velocity.X = 0; }
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
            //prevPosition = position;
        }
        
        public void CollideBump(Tile tile) {
            // get angle between tile and player centers
            float centerAngle = MathF.Atan2((position.Y + dimensions.Y/2) - (tile.Position.Y + 20), // hardcoded the width and height of tiles
                                      (position.X + dimensions.X/2) - (tile.Position.X + 20));

            float angleBoundary = MathF.Atan2((dimensions.Y + 40), (dimensions.X + 40)); // hardcoded the width and height
    
            // top
            if(centerAngle > angleBoundary && centerAngle < MathHelper.Pi - angleBoundary)
            {
                position.Y = tile.Position.Y - dimensions.Y;
                velocity.Y = 0;
            }

            // left
            if (centerAngle > MathHelper.Pi - angleBoundary && centerAngle < angleBoundary - MathHelper.Pi)
            {
                position.X = tile.Position.X - dimensions.X;
                velocity.X = 0;
            }

            // bottom
            if (centerAngle > angleBoundary - MathHelper.Pi && centerAngle < 0 - angleBoundary)
            {
                position.Y = tile.Position.Y + 40; // hardcoded height
                velocity.Y = 0;
            }
            // right
            if (centerAngle > 0 - angleBoundary && centerAngle < angleBoundary)
            {
                position.X = tile.Position.X + 40; // hardcoded width
                velocity.X = 0;
            }
        }

        /// <summary>
        /// player <-> wire
        /// Takes in input from user (interact) Grabs the end of a wire if close enough, releases the wire, or connectes it to a power node if close enough
        /// moves the end of the wire with player if holding wire
        /// </summary>
        public void InteractWire()
        {
            //holding wire
        //move wire with player
        //if user presses interact and near a power node then place the wire on power node
        //else drop the wire

            //not holding wire
        //if near the end of wire pick it up
        }
    }
}
