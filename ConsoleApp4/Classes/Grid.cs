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

        public int GetBestScenicScene()
        {
            var bestScenicScene = -1;

            foreach(var tree in Trees) // "en-dimensionell" titt
            {
                // Kolla rekursivt från varje träd för att hitta det med bäst score
                var scoreX = GetVisibleTreesFromTree(tree, tree.Length, x => x + 1, y => y);
                var scoreY = GetVisibleTreesFromTree(tree, tree.Length, x => x, y => y + 1);
                var scoreXBack = GetVisibleTreesFromTree(tree, tree.Length, x => x - 1, y => y);
                var scoreYBack = GetVisibleTreesFromTree(tree, tree.Length, x => x, y => y - 1);

                var currentScenicScore = scoreX * scoreY * scoreXBack * scoreYBack;

                if (currentScenicScore > bestScenicScene)
                    bestScenicScene = currentScenicScore;
            }

            return bestScenicScene;
        }

        private int GetVisibleTreesFromTree(Tree tree, int sourceTreeLength, Func<int, int> xFunc, Func<int, int> yFunc)
        {
            // Jag ser ju alltid nästa, undantag nedan
            var visibleFromHere = 1;

            // returnera noll om är på kanten
            var nextTree = GetTree(xFunc(tree.X), yFunc(tree.Y));
            if (nextTree == null)
                return 0;

            // Om nästa träd är lägre än det jag står på samt ursprungsträdet
            if (nextTree.Length < tree.Length || sourceTreeLength > nextTree.Length)
                return visibleFromHere += GetVisibleTreesFromTree(nextTree, sourceTreeLength, xFunc, yFunc);

            return visibleFromHere;
        }
    }
}
