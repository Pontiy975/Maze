using System.Collections.Generic;
using UnityEngine;

namespace Maze.Core.Generators
{
    public class PrimsMazeGenerator : IMazeGenerator
    {
        public void Generate(MazeNode[,] grid, Vector2Int size)
        {
            HashSet<MazeNode> inMaze = new HashSet<MazeNode>();
            List<MazeNode> frontier = new List<MazeNode>();

            MazeNode start = grid[Random.Range(0, size.x), Random.Range(0, size.y)];
            inMaze.Add(start);

            AddFrontier(start, grid, size, inMaze, frontier);

            while (frontier.Count > 0)
            {
                int idx = Random.Range(0, frontier.Count);
                MazeNode current = frontier[idx];
                frontier.RemoveAt(idx);

                List<MazeNode> neighborsInMaze = new List<MazeNode>();
                foreach (var neighbor in GetNeighbors(grid, size, current.Position))
                    if (inMaze.Contains(neighbor))
                        neighborsInMaze.Add(neighbor);

                if (neighborsInMaze.Count > 0)
                {
                    MazeNode connectTo = neighborsInMaze[Random.Range(0, neighborsInMaze.Count)];
                    current.Connect(connectTo);
                }

                inMaze.Add(current);
                AddFrontier(current, grid, size, inMaze, frontier);
            }
        }

        private void AddFrontier(MazeNode node, MazeNode[,] grid, Vector2Int size, HashSet<MazeNode> inMaze, List<MazeNode> frontier)
        {
            foreach (var neighbor in GetNeighbors(grid, size, node.Position))
            {
                if (!inMaze.Contains(neighbor) && !frontier.Contains(neighbor))
                    frontier.Add(neighbor);
            }
        }

        private List<MazeNode> GetNeighbors(MazeNode[,] grid, Vector2Int size, Vector2Int pos)
        {
            List<MazeNode> neighbors = new List<MazeNode>();
            void TryAdd(int x, int y)
            {
                if (x >= 0 && x < size.x && y >= 0 && y < size.y)
                    neighbors.Add(grid[x, y]);
            }

            TryAdd(pos.x - 1, pos.y);
            TryAdd(pos.x + 1, pos.y);
            TryAdd(pos.x, pos.y - 1);
            TryAdd(pos.x, pos.y + 1);

            return neighbors;
        }
    }
}
