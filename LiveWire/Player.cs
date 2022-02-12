using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

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
        private AnimState currentState;
        private Rectangle position;
        private Vector2 velocity;
        private bool isHoldingWire;



        // --- PROPERTIES ---
        public Rectangle Position
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

        public bool IsHoldingWire
        {
            get { return isHoldingWire; }
            set { isHoldingWire = value; }
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
    }
}
