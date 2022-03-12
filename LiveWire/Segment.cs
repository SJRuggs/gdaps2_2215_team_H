using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LiveWire
{

    /// <summary>
    /// The segment class defeines the individual parts of a wire, which interacts with the physical enviroment
    /// </summary>

    public class Segment
    {
        // --- VARIABLE DELCARATIONS ---
        private Vector2 node1;
        private Vector2 node2;
        private Texture2D segment;



        // --- PROPERTIES ---
        public Vector2 Node1
        {
            get { return node1; }
            set { node1 = value; }
        }
        public Vector2 Node2
        {
            get { return node2; }
            set { node2 = value; }
        }



        // --- CONSTRUCTOR ---
        public Segment(Vector2 node1, Vector2 node2)
        {
            this.node1 = node1;
            this.node2 = node2;
        }



        // --- METHODS ---
        public float Radians()
        {
            // returns angle in radians
            double result = Math.Atan2(node2.Y - node1.Y, node2.X - node1.X);
            if(node2.X < node1.X)
            {
                result += Math.PI;
                if (node2.Y < node1.Y) { result += Math.PI / 2; }
            }
            else
            {
                if (node2.Y > node1.Y) { result += Math.PI / 2; }
            }
            return (float)result;
        }

        // distance between nodes
        public double Distance()
        {
            return Vector2.Distance(node1, node2);
        }

        // calls all relevant methods
        public void Update(TileParent[,] board, Wire wire)
        {
            LimitSegment(wire);
            DetectCollision(board, wire);
        }

        // limits wire based on total length and max length
        public void LimitSegment(Wire wire)
        {
            if(wire.GetTotalLength() > wire.MaxLength)
            {
                Vector2 extraVector = new Vector2();
                double extra = wire.GetTotalLength() - wire.MaxLength;
                extraVector.Y = (float)(Math.Sin(Math.Atan2(node2.Y - node1.Y, node2.X - node1.X)) * extra);
                extraVector.X = (float)Math.Sqrt(Math.Pow(extraVector.Y, 2) - Math.Pow(extra, 2));
                if (node2.X > node1.X) { node2.X -= extraVector.X; }
                else { node2.X += extraVector.X; }
                if (node2.Y > node1.Y) { node2.Y -= extraVector.Y; }
                else { node2.Y += extraVector.Y; }
            }
        }

        // detects a collision on the segment with tiles that block the wire
        public void DetectCollision(TileParent[,] board, Wire wire)
        {
            int stepCount;
            Vector2 direction;
            Vector2 stepHeight;
            Vector2 checkPosition;

            stepCount = (int)Math.Max(Math.Abs(Node2.X - Node1.X), Math.Abs(Node2.Y - Node1.Y));
            direction = new Vector2((node2.X - node1.X) / Math.Abs(Node2.X - Node1.X), (node2.Y - node1.Y) / Math.Abs(Node2.Y - Node1.Y));
            stepHeight = new Vector2(Math.Abs(Node2.X - Node1.X) / stepCount, Math.Abs(Node2.Y - Node1.Y) / stepCount);
            checkPosition = node1;

            for (int i = 0; i < stepCount; i++)
            {
                // updates the position of the checker
                checkPosition.X += direction.X * stepHeight.X;
                checkPosition.Y += direction.Y * stepHeight.Y;

                // if inside a tile that blocks the wire...
                if (board[(int)(checkPosition.X / 40), (int)(checkPosition.Y / 40)].BlocksWire)
                {
                    // set node2 point
                    this.node2.X = checkPosition.X - (checkPosition.X % 40);
                    this.node2.Y = checkPosition.Y - (checkPosition.Y % 40);
                    if (checkPosition.X % 40 > 21) { this.node2.X += 40; }
                    if (checkPosition.Y % 40 > 21) { this.node2.Y += 40; }

                    // create new segment on wire
                    wire.AddSegment(new Segment(node2, wire.Player.Position));
                    i = stepCount;
                }

                // if inside a tile that interacts with the wire...
                if (board[(int)(checkPosition.X / 40), (int)(checkPosition.Y / 40)].InteractsWire)
                {
                    board[(int)(checkPosition.X / 40), (int)(checkPosition.Y / 40)].PlayerInteract(wire.Player);
                    i = stepCount;
                }
            }
        }

        // handles the creation of a new segment within the wire
        public void newSegment(Tile tile, Wire wire, float x, float y)
        {
            // right side of tile
            if (tile.Position.X + tile.Position.Width < x)
            {
                // top of tile
                if (tile.Position.Y + tile.Position.Height > y)
                {
                    // create a new node at the top right of the tile
                    node2.X = tile.Position.X + tile.Position.Width;
                    node2.Y = tile.Position.Y;
                }
                // bottom of tile
                else
                {
                    // create a new node at the bottom right of the tile
                    node2.X = tile.Position.X + tile.Position.Width;
                    node2.Y = tile.Position.Y + tile.Position.Height;
                }
            }
            // left side of tile
            else
            {
                // top of tile
                if (tile.Position.Y + tile.Position.Height > y)
                {
                    // create a new node at the top left of the tile
                    node2.X = tile.Position.X;
                    node2.Y = tile.Position.Y;
                }
                // bottom of tile
                else
                {
                    // create a new node at the bottom left of the tile
                    node2.X = tile.Position.X;
                    node2.Y = tile.Position.Y + tile.Position.Height;
                }
            }
            // create a new segment at the end of the wire's segment list
            wire.Wires.Add(new Segment(node2, wire.Player.Position));
        }

        // draws the segment between the two nodes
        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphics, Wire wire)
        {
            segment = new Texture2D(graphics, 1, 1);
            segment.SetData(new[] { new Color(255 - (wire.GetTotalLength() / wire.MaxLength * 255), wire.GetTotalLength() / wire.MaxLength * 255, 0) });
            spriteBatch.Draw(
                segment,
                new Rectangle((int)node1.X, (int)node1.Y, 1, (int)this.Distance()),
                null,
                Color.White,
                this.Radians(),
                Vector2.Zero,
                SpriteEffects.None,
                0
                );
        }
    }
}
