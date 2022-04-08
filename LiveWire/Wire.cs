﻿using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiveWire
{

    /// <summary>
    /// TODO: Add Summary
    /// </summary>

    public class Wire
    {
        // --- VARIABLE DELCARATIONS ---
        private float maxLength = 200;
        private List<Segment> wires;
        private Player player;



        // --- PROPERTIES ---
        public float MaxLength
        {
            get { return maxLength; }
            set { maxLength = value; }
        }

        public List<Segment> Wires
        {
            get { return wires; }
        }

        public Player Player
        {
            get { return player; }
        }



        // --- CONSTRUCTOR ---
        public Wire(Player player)
        {
            wires = new List<Segment>();
            this.player = player;
        }



        // --- METHODS ---

        // collects the lengths of each segment and returns it
        public float GetTotalLength()
        {
            float totalLength = 0;
            foreach (Segment s in wires) { totalLength += (float)s.Distance(); }
            return totalLength;
        }

        // adds a new segment to wires
        public void AddSegment(Segment s)
        {
            wires.Add(s);
        }

        // detects and trims irrelevant nodes
        public void DetectTrim()
        {
            if (wires.Count > 1)
            {
                rads = wires[i].Radians();
                rads2 = wires[i + 1].Radians();
                // wire on Q1
                if (wires[i].Node2.X % 40 > 20 && wires[i].Node2.Y % 40 < 21)
                {
                    // pulling to Q4
                    if (rads > Math.PI)
                    {
                        if (rads > rads2)
                        {
                            TrimLastSegment();
                        }
                    }
                    // pulling to Q2
                    else
                    {
                        if (rads < rads2)
                        {
                            TrimLastSegment();
                        }
                    }
                }
                // wire on Q2
                else if (wires[i].Node2.X % 40 > 20 && wires[i].Node2.Y % 40 > 20)
                {
                    // pulling to Q3
                    if (rads > Math.PI)
                    {
                        if (rads < rads2)
                        {
                            TrimLastSegment();
                        }
                    }
                    // pulling to Q1
                    else
                    {
                        if (rads > rads2)
                        {
                            TrimLastSegment();
                        }
                    }
                }
                // wire on Q3
                else if (wires[i].Node2.X % 40 < 21 && wires[i].Node2.Y % 40 > 20)
                {
                    // pulling to Q4
                    if (rads > Math.PI)
                    {
                        if (rads < rads2)
                        {
                            TrimLastSegment();
                        }
                    }
                    // pulling to Q2
                    else
                    {
                        if (rads > rads2)
                        {
                            TrimLastSegment();
                        }
                    }
                }
                // wire on Q4
                else
                {
                    // pulling to Q3
                    if (rads > Math.PI)
                    {
                        if (rads > rads2)
                        {
                            TrimLastSegment();
                        }
                    }
                    // pulling to Q1
                    else
                    {
                        if (rads < rads2)
                        {
                            TrimLastSegment();
                        }
                    }
                }
            }
        }

        // detects collision for each segment
        public void Update(TileParent[,] board)
        {
            wires[wires.Count - 1].Update(board, this, player);
            DetectTrim();
            //wires[wires.Count - 1].LimitSegment(this, player);
        }

        // trims the last segment and updates the new end to find the player
        public void TrimLastSegment()
        {
            wires.RemoveAt(wires.Count - 1);
            wires[wires.Count - 1].Node2 = player.Position;
        }


        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            foreach (Segment s in wires) { s.Draw(spriteBatch, graphics, this); }
        }
    }
}
