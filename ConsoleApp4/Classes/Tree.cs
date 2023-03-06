using System;

namespace ConsoleApp4.Classes
{
    class Tree
    {
        public Tree(int x, int y, int len)
        {
            X = x;
            Y = y;
            Length = len;
            Guid = Guid.NewGuid();
        }

        public int X { get; }
        public int Y { get; }
        public int Length { get; }
        public Guid Guid { get; }
    }
}
