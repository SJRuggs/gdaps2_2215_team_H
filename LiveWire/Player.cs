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
        private Vector2 velocity;
        private const float speed = 5;
        private const float gravity = 10;

        // interaction variables
        private bool isHoldingWire;
        private Wire holdingWire;

        // user input keys
        private const Keys left = Keys.Left;
        private const Keys right = Keys.Right;
        private const Keys jump = Keys.Up;
        private const Keys interact = Keys.Z;



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
        //change horizontal velocity with left and right inputs
        //change horizontal velocity with speed

            //vertical velocity
        //update vertical velocity with jump force
        //update vertical velocity with gravity

            //position/collision
        //update position with velocity
        //if position colliding with block, adjust position to not be in block and adjust velocity to not move you in to block
        //is collision reactive or proactive?
        }
        
        public void CollideBump(Tile tile) { }

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
