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
            return (float)(Math.Atan2(node2.Y - node1.Y, node2.X - node1.X) - Math.PI / 2);
        }

        // distance between nodes
        public double Distance()
        {
            return Vector2.Distance(node1, node2);
        }

        // calls all relevant methods
        public bool Update(TileParent[,] board, Wire wire, Player player)
        {
            return DetectCollision(board, wire);
        }

        // limits wire based on total length and max length
        public void LimitSegment(Wire wire, Player player)
        {
            player.Position = new Vector2(
                node1.X + (node2.X - node1.X) * Math.Min(wire.GetTotalLength() / wire.MaxLength, 1),
                node1.Y + (node2.Y - node1.Y) * Math.Min(wire.GetTotalLength() / wire.MaxLength, 1));
        }

        // detects a collision on the segment with tiles that block the wire
        public bool DetectCollision(TileParent[,] board, Wire wire)
        {
            float x = node2.X;
            float y = node2.Y;

            for (int i = 0; i < Math.Max(Math.Abs(node2.X - node1.X), Math.Abs(node2.Y - node1.Y)); i++)
            {
                // tall slope
                if (Math.Abs(node2.Y - node1.Y) > Math.Abs(node2.X - node1.X))
                {
                    if (node2.Y > node1.Y) { y--; }
                    else { y++; }
                    if (node2.X > node1.X) { x -= Math.Abs((node2.X - node1.X) / (node2.Y - node1.Y)); }
                    else { x += Math.Abs((node2.X - node1.X) / (node2.Y - node1.Y)); }
                }

                // long slope
                else
                {
                    if (node2.Y > node1.Y) { y -= Math.Abs((node2.Y - node1.Y) / (node2.X - node1.X)); }
                    else { y += Math.Abs((node2.Y - node1.Y) / (node2.X - node1.X)); }
                    if (node2.X > node1.X) { x--; }
                    else { x++; }
                }

                  if (board[(int)(y / 40), (int)(x / 40)].BlocksWire)
                {
                    if (x % 40 < 21) { x -= (x % 40) + 3; }
                    else { x += 40 - (x % 40) + 3; }
                    if (y % 40 < 21) { y -= (y % 40) + 3; }
                    else { y += 40 - (y % 40) + 3; }

                    newSegment(board, wire, new Vector2(x, y));
                    return true;
                }
            }
            return false;
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
