﻿using System;
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
        public void Update(TileParent[,] board, Wire wire, Player player)
        {
            LimitSegment(wire, player);
            DetectCollision(board, wire);
        }

        // limits wire based on total length and max length
        public void LimitSegment(Wire wire, Player player)
        {
            if(wire.GetTotalLength() > wire.MaxLength)
            {
                player.Position = player.PrevPosition;
            }
        }

        // detects a collision on the segment with tiles that block the wire
        public void DetectCollision(TileParent[,] board, Wire wire)
        {
            Vector2 loc = new Vector2();
            bool found = false;

            float y;
            float yChange;
            if (node1.Y < node2.Y)
            {
                yChange = 1;
            }
            else
            {
                yChange = -1;
            }
            if (node1.X < node2.X)
            {
                y = node1.Y;
                for (int x = (int)node1.X + 1; x < node1.X + Math.Abs(node1.X - node2.X); x++)
                {
                    y += yChange * (Math.Abs(node1.Y - node2.Y) / Math.Abs(node1.X - node2.X));
                    if (board[(int)(y / 40), x / 40].BlocksWire)
                    {
                        found = true;
                        loc = new Vector2(x, y);
                    }
                    else if (!board[(int)(y / 40), x / 40].BlocksWire && found)
                    {
                        newSegment((Tile)board[(int)(y / 40), x / 40], wire, loc);
                        return;
                    }
                }
            }
            else
            {
                y = node2.Y;
                for (int x = (int)node1.X - 1; x > node1.X + Math.Abs(node1.X - node2.X); x--)
                {
                    y += yChange * (Math.Abs(node1.Y - node2.Y) / Math.Abs(node1.X - node2.X));
                    if (board[(int)(y / 40), x / 40].BlocksWire)
                    {
                        found = true;
                        loc = new Vector2(x, y);
                    }
                    else if (!board[(int)(y / 40), x / 40].BlocksWire && found) 
                    {
                        newSegment((Tile)board[(int)(y / 40), x / 40], wire, loc);
                        return;
                    }
                }
            }
        }

        // handles the creation of a new segment within the wire
        public void newSegment(Tile tile, Wire wire, Vector2 pos)
        {
            // right side of tile
            if (tile.Position.X + tile.Position.Width / 2 < pos.X)
            {
                // top of tile
                if (tile.Position.Y + tile.Position.Height / 2 > pos.Y)
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
                if (tile.Position.Y + tile.Position.Height / 2 > pos.Y)
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
