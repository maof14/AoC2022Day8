using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp4.Classes
{
    class Grid
    {
        public Grid(int length)
        {
            Length = length;
        }

        public List<Tree> Trees { get; } = new List<Tree>();
        public int Length { get; }

        public void AddRows(params string[] rows)
        {
            var x = 0; // columns
            foreach (var row in rows)
            {
                var y = 0; // Rows
                var rowChars = row.ToCharArray();
                foreach (var c in rowChars)
                {
                    Trees.Add(new Tree(x, y, int.Parse(c.ToString())));
                    y++;
                }
                x++;
            }
        }

        public IEnumerable<Tree> GetVisibleTreesByColumn(int x, bool backwards = false)
        {
            var visibleTrees = new List<Tree>();

            // Kolla hela tiden på kolumn x, neråt på y
            var enumerable = Enumerable.Range(0, Length);
            if (backwards)
                enumerable = enumerable.Reverse();

            visibleTrees.Add(GetTree(x, enumerable.First()));

            var highest = -1;

            foreach(var y in enumerable)
            {
                var current = GetTree(x, y);
                if (current.Length > highest)
                    highest = current.Length;

                var yPos = !backwards ? y + 1 : y - 1;
                var nextTree = GetTree(x, yPos);
                if (nextTree == null)
                    break;

                if (nextTree.Length > highest)
                    visibleTrees.Add(nextTree);
            }

            return visibleTrees;
        }

        public IEnumerable<Tree> GetVisibleTreesByRow(int y, bool backwards = false)
        {
            var visibleTrees = new List<Tree>();

            // Kolla hela tiden på kolumn x, neråt på y
            var enumerable = Enumerable.Range(0, Length);
            if (backwards)
                enumerable = enumerable.Reverse();

            visibleTrees.Add(GetTree(enumerable.First(), y));

            var highest = -1;

            foreach (var x in enumerable)
            {
                var current = GetTree(x, y);
                if (current.Length > highest)
                    highest = current.Length;

                var xPos = !backwards ? x + 1 : x - 1;
                var nextTree = GetTree(xPos, y);
                if (nextTree == null)
                    break;

                if (nextTree.Length > highest)
                    visibleTrees.Add(nextTree);
            }

            return visibleTrees;
        }

        public Tree GetTree(int x, int y) {
            return Trees.FirstOrDefault(tree => tree.X == x && tree.Y == y);
        }
    }
}
