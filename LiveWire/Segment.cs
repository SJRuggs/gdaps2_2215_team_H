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
        private Ray ray;



        // --- PROPERTIES ---
        public Vector2 Node1
        {
            get { return node1; }
            set 
            {
                node1 = value; 
                ray.Position.X = value.X; 
                ray.Position.Y = value.Y;
            }
        }
        public Vector2 Node2
        {
            get { return node2; }
            set
            {
                node1 = value;
                ray.Direction.X = value.X - node1.X;
                ray.Direction.Y = value.Y - node1.Y;
            }
        }



        // --- CONSTRUCTOR ---
        public Segment(Vector2 node1, Vector2 node2)
        {
            this.node1 = node1;
            this.node2 = node2;
            ray = new Ray(new Vector3(node1.X, node1.Y, 0), new Vector3(node2.X, node2.Y, 0));
        }



        // --- METHODS ---
        public float Radians()
        {
            // returns angle in radians
            return (float)(Math.Atan2(node2.Y - node1.Y, node2.X - node1.X) - Math.PI / 2);
        }

        // distance between nodes
        public double Distance()
        {
            return Vector2.Distance(node1, node2);
        }

        // calls all relevant methods
        public void Update(TileParent[,] board, Wire wire, Player player)
        {
            DetectCollision(board, wire);
        }

        // limits wire based on total length and max length
        public void LimitSegment(Wire wire, Player player)
        {
            player.Position = new Vector2(
                node1.X + (node2.X - node1.X) * Math.Min(wire.GetTotalLength() / wire.MaxLength, 1),
                node1.Y + (node2.Y - node1.Y) * Math.Min(wire.GetTotalLength() / wire.MaxLength, 1));
        }

        // detects a collision on the segment with tiles that block the wire
        public void DetectCollision(TileParent[,] board, Wire wire)
        {
            for (int i = 0; i < ray.Direction.X * ray.Direction.Y; i++)
            {
                if (ray.Intersects(new BoundingBox(
                    new Vector3(
                        board[(int)(i % ray.Direction.X / 40), (int)(i / ray.Direction.Y / 40)].Position.X,
                        board[(int)(i % ray.Direction.X / 40), (int)(i / ray.Direction.Y / 40)].Position.Y,
                        0),
                    new Vector3(
                        board[(int)(i % ray.Direction.X / 40), (int)(i / ray.Direction.Y / 40)].Position.Width,
                        board[(int)(i % ray.Direction.X / 40), (int)(i / ray.Direction.Y / 40)].Position.Height,
                        0))) != null)
                {
                    newSegment(board, wire, new Vector2(i % ray.Direction.X, (int)(i / ray.Direction.Y)));
                    return;
                }
            }
        }

        // handles the creation of a new segment within the wire
        public void newSegment(TileParent[,] board, Wire wire, Vector2 enter)
        {
            node2 = enter;
            wire.Wires.Add(new Segment(enter, wire.Player.Center()));
        }

        // draws the segment between the two nodes
        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphics, Wire wire)
        {
            segment = new Texture2D(graphics, 1, 1);
            segment.SetData(new[] { Color.Green });
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
