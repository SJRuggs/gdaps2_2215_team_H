using System;
using System.Collections.Generic;
using System.Text;

namespace LiveWire
{

    /// <summary>
    /// TODO: Add Summary
    /// </summary>

    class Wire
    {
        // --- VARIABLE DELCARATIONS ---
        private double maxLength = 50;
        private List<Segment> wires;
        private Player player;



        // --- PROPERTIES ---
        public double MaxLength
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
        public Wire()
        {
            wires = new List<Segment>();
        }



        // --- METHODS ---

        // collects the lengths of each segment and returns it
        public double GetTotalLength()
        {
            double totalLength = 0;
            foreach (Segment s in wires) { totalLength += s.Distance(); }
            return totalLength;
        }

        // adds a new segment to wires
        public void AddSegment(Segment s)
        {
            wires.Add(s);
        }
    }
}
