using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace LiveWire
{

    /// <summary>
    /// TODO: Add Summary
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
        public double getAngle()
        {
            // returns angle in radians
            return Math.Acos(Math.Abs(node1.Y - node2.Y) / Math.Abs(node1.X - node2.X));
        }

        public double getDistance()
        {
            return Vector2.Distance(node1, node2);
        }
    }
}
