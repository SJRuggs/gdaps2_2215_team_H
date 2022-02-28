using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace LiveWire
{

    /// <summary>
    /// The segment class defeines the individual parts of a wire, which interacts with the physical enviroment
    /// </summary>

    class Segment
    {
        // --- VARIABLE DELCARATIONS ---
        private Vector2 node1;
        private Vector2 node2;



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
        public Segment(int x1, int y1, int x2, int y2)
        {
            node1 = new Vector2(x1, y1);
            node2 = new Vector2(x2, y2);
        }



        // --- METHODS ---
        public double Angle()
        {
            // returns angle in radians
            return Math.Acos(Math.Abs(node1.Y - node2.Y) / Math.Abs(node1.X - node2.X));
        }

        public double Distance()
        {
            return Vector2.Distance(node1, node2);
        }

        // detects a collision on the segment with tiles that block the wire
        public void DetectCollision(Tile[,] board, Wire wire)
        {
            float stepHeight = Math.Abs(node1.Y - node2.Y);

            // moving right
            if (node1.X < node2.X)
            {
                // moving down
                if (node1.Y < node2.Y)
                {
                    for (float x = node1.X; x < (int)node2.X; x++)
                        if (board[(int)((node1.X + x) / 40), (int)(node1.Y + x * stepHeight)].BlocksWire)
                        {
                            // TODO: calculate position of the new node
                        }
                }
                // moving up
                else
                {
                    for (float x = node1.X; x < (int)node2.X; x++)
                        if (board[(int)((node1.X + x) / 40), (int)(node1.Y - x * stepHeight)].BlocksWire)
                        {
                            // TODO: calculate position of the new node
                        }
                }
            }
            // moving left
            else
            {
                // moving down
                if (node1.Y < node2.Y)
                {
                    for (float x = node1.X; x < (int)node2.X; x++)
                        if (board[(int)((node1.X - x) / 40), (int)(node1.Y + x * stepHeight)].BlocksWire)
                        {
                            // TODO: calculate position of the new node
                        }
                }
                // moving up
                else
                {
                    for (float x = node1.X; x < (int)node2.X; x++)
                        if (board[(int)((node1.X - x) / 40), (int)(node1.Y - x * stepHeight)].BlocksWire)
                        {
                            // TODO: calculate position of the new node
                        }
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
            wire.Wires.Add(new Segment((int)node2.X, (int)node2.Y, (int)wire.Player.Position.X, (int)wire.Player.Position.Y));
        }
    }
}
