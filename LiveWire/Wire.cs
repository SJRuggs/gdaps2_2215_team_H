using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
            set { player = value; }
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
                if (Math.Abs(wires[wires.Count - 1].Radians() - wires[wires.Count - 2].Radians()) < 3 /
                    Vector2.Distance(wires[wires.Count - 1].Node2, wires[wires.Count - 1].Node1))
                {
                    TrimLastSegment();
                }
            }
        }

        // detects collision for each segment
        public bool Update(TileParent[,] board)
        {
            DetectTrim();
            return wires[wires.Count - 1].Update(board, this, player);
            //wires[wires.Count - 1].LimitSegment(this, player);
        }

        // trims the last segment and updates the new end to find the player
        public void TrimLastSegment()
        {
            wires.RemoveAt(wires.Count - 1);
            wires[wires.Count - 1].Node2 = player.Center();
        }


        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            foreach (Segment s in wires) { s.Draw(spriteBatch, graphics, this); }
        }
    }
}
