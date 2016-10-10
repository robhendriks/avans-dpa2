using System;

namespace DPA_Musicsheets.Utility
{
    public struct Range
    {
        public int Start { get; set; }
        public int End { get; set; }

        public Range(int a, int b)
        {
            Start = Math.Min(a, b);
            End = Math.Max(a, b);
        }

        public bool In(int value)
        {
            return (value >= Start && value <= End);
        }
    }
}
