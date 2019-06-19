using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.DataStructures
{
    public class Point
    {
        public Position Position;
        public byte? Value;

        public Point(byte y, byte x)
        {
            this.Position = new Position(y, x);
            this.Value = null;
        }
    }

    public class Position
    {
        public byte X;
        public byte Y;

        public Position(byte y, byte x)
        {
            this.Y = y;
            this.X = x;
        }
    }

    public class Area
    {
        public AreaNumber AreaPosition;
        public bool IsSolved = false;
        public Position StartPosition;

        public Area(AreaNumber areaPosition, Position startPosition)
        {
            this.AreaPosition = areaPosition;
            this.StartPosition = startPosition;
            this.IsSolved = false;
        }
    }

    public enum AreaNumber
    {
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9
    }
}
