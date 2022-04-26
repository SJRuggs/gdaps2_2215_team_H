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
        private Texture2D sprite;

        // physics variables
        private Vector2 position;
        private Vector2 prevPosition;
        private Vector2 velocity;
        private float speed; // how fast you move in the x axis
        private float jumpForce; // how strong your jump is
        private float gravity; // how strong gravity is
        private Rectangle box;
        private Vector2 dimensions;
        private SortedList<float,TileParent> closestTiles;
        //private bool onGround;

        private int coyoteFrame; 
        private int maxCoyoteFrames; // the amount of frames you have after leaving the ground to input a jump

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
        public Vector2 Dimensions
        {
            get { return dimensions; }
            set { dimensions = value; }
        }

        public Vector2 PrevPosition
        {
            get { return prevPosition; }
            set { prevPosition = value; }
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
            this.sprite = playerSprite;
            this.dimensions = new Vector2(playerSprite.Width,playerSprite.Height);
            box = new Rectangle(position.ToPoint(), dimensions.ToPoint());
            prevPosition = position;
            left = Keys.Left;
            right = Keys.Right;
            jump = Keys.Up;
            interact = Keys.Space;

            // can jump 12 blocks in distance and 3 blocks high
            speed = 5f;
            jumpForce = 5f;
            gravity = 0.1f;
            maxCoyoteFrames = 6;
            coyoteFrame = 0;
            closestTiles = new SortedList<float, TileParent>();
        }
        // --- METHODS ---

        // updates the Player animation by its animstate
        /*public void UpdateFSM()
        {
            switch (currentState)
            {
                // TODO: Add cases
            }
        }*/

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, new Rectangle(position.ToPoint(), dimensions.ToPoint()), Color.White);
        }

        /// <summary>
        /// player <-> enviroment
        /// Takes in input from user (left,right,jump) and moves the player accordingly to physics (gravity, colliding with ground, ground friction) 
        /// </summary>
        public void PlayerMovement(KeyboardState kbState, KeyboardState prevKbState, TileParent[,] board)
        {
            //horizontal velocity (horizontal movement can feel better if, velocity doesnt get reset but instead use a friction variable to slow down the character when on the ground,
            //use acceleration have some max velocity variable to 
            
            //reset horizontal velocity
            velocity.X = 0;

            //change horizontal velocity with left and right inputs
            if (kbState.IsKeyDown(left)){ velocity.X = -1; }
            if (kbState.IsKeyDown(right)){ velocity.X = 1; }            
            
            //change horizontal velocity with speed
            velocity.X *= speed;

            //vertical velocity
            //update vertical velocity with jump force
            if (kbState.IsKeyDown(jump) && prevKbState.IsKeyUp(jump) && (coyoteFrame < maxCoyoteFrames)){ 
                velocity.Y = -jumpForce;
                coyoteFrame = maxCoyoteFrames + 1;
            }
            //update vertical velocity with gravity
            velocity.Y += gravity;

            //position/collision
            //update position with velocity
            position += velocity;
            //if position colliding with block, adjust position to not be in block and adjust velocity to not move you in to block
            //is collision reactive or proactive?
            //reactive
            //loop through all the tiles on screen

            //onGround = false;
            coyoteFrame++;

            //Do collision from closest block outwards
            closestTiles.Clear();
            for (int i = 0; i < board.GetLength(0); i++) 
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    TileParent tile = null;
                    if (board[i,j] is TileParent)
                    {
                        tile = (TileParent)board[i,j];
                    }
                    if(tile.BlocksPLayer && (tile != null) && Vector2.Distance(tile.Position.Center.ToVector2(), Center()) < 80)
                    {
                        if (!closestTiles.ContainsKey(Vector2.Distance(tile.Position.Center.ToVector2(), Center())))
                        {
                            closestTiles.Add(Vector2.Distance(tile.Position.Center.ToVector2(), Center()), tile);
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("same key generated");
                        }
                        // ^will break if two close blocks are the same dist away since that creates the same key
                        // insert tiles into an array based on what the distance away is
                    }                    
                }
            }           
            foreach(float dist in closestTiles.Keys) { 
                if (new Rectangle(position.ToPoint(), dimensions.ToPoint()).Intersects(closestTiles[dist].Position))
                {
                    CollideBump(closestTiles[dist]);
                }
            }

            //saving last frame position of player
            prevPosition = position;
        }
        
        public void CollideBump(TileParent tile) {
            // get angle between tile and player centers
            float centerAngle = MathF.Atan2((position.Y + dimensions.Y/2) - (tile.Position.Y + tile.Position.Height/2),
                                      (position.X + dimensions.X/2) - (tile.Position.X + tile.Position.Width/2));

            float angleBoundary = MathF.Atan2((dimensions.Y + tile.Position.Height), (dimensions.X + tile.Position.Width));
            //System.Diagnostics.Debug.WriteLine(angleBoundary * 180f / MathHelper.Pi);

            // run into top of block
            if (centerAngle > angleBoundary - MathHelper.Pi && centerAngle < 0 - angleBoundary)
            {
                //System.Diagnostics.Debug.WriteLine("ran into top of block");
                position.Y = tile.Position.Y - dimensions.Y;
                if(velocity.Y > 0) { velocity.Y = 0; }
                coyoteFrame = 0;
                //onGround = true;
                //velocity.Y = 0;
            }

            // run into bottom of block
            if (centerAngle > angleBoundary && centerAngle < MathHelper.Pi - angleBoundary)
            {
                //System.Diagnostics.Debug.WriteLine("ran into bottom of block");
                //System.Diagnostics.Debug.WriteLine(centerAngle * 180f / MathHelper.Pi);
                //System.Diagnostics.Debug.WriteLine(velocity.Y);
                position.Y = tile.Position.Y + tile.Position.Height;
                if (velocity.Y < 0) { velocity.Y = 0; }
                //velocity.Y = 0;
            }

            // run into left of block
            if ((centerAngle > MathHelper.Pi - angleBoundary && centerAngle <= MathHelper.Pi) || (centerAngle >= - MathHelper.Pi) && (centerAngle < angleBoundary-MathHelper.Pi))
            {
                //System.Diagnostics.Debug.WriteLine("ran into left of block");
                position.X = tile.Position.X - dimensions.X;
                velocity.X = 0;
            }
            
            // run into right of block
            if (centerAngle >= 0 - angleBoundary && centerAngle <= angleBoundary)
            {
                //System.Diagnostics.Debug.WriteLine("ran into right of block");
                //System.Diagnostics.Debug.WriteLine(centerAngle*180f/MathHelper.Pi);
                position.X = tile.Position.X + tile.Position.Width;
                velocity.X = 0;
            }

        }

        /// <summary>
        /// player <-> wire
        /// Takes in input from user (interact) and connectes a wire to a power node if close enough
        /// Unused: grabs the end of a wire if close enough, releases the wire
        /// </summary>
        public void InteractWire(KeyboardState kbState, KeyboardState prevKbState, List<Machine> machines)
        {
            
            // find the closest machine
            float distToMachine = float.MaxValue;
            Machine closestMachine = null;
            
            foreach(Machine machine in machines){
                if(Vector2.Distance(Center(),  machine.Center) < distToMachine){
                    distToMachine = Vector2.Distance(Center(),  machine.Center);
                    closestMachine = machine;
                }
            }

            if (kbState.IsKeyDown(interact) && !prevKbState.IsKeyDown(interact))
            {               
                closestMachine.PlayerInteract(this);
            }

            /*

            // Unused; the Player is unable to drop and pick up the wire in the current build

            if (isHoldingWire)
            {
                //holding wire
                //move wire with player
                //if user presses interact and near a power node then place the wire on power node
                //else drop the wire
            }
            else
            {
                //not holding wire
                //if near the end of wire pick it up
            }
            */
        }

        // NOT TRUE CENTER, center of player's head
        public Vector2 Center()
        {
            return new Vector2(position.X + dimensions.X / 2, position.Y + dimensions.Y / 3);
        }
    }
}
