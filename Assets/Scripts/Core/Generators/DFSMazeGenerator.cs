using System.Collections.Generic;
using UnityEngine;

namespace Maze.Core.Generators
{
    public class DFSMazeGenerator : IMazeGenerator
    {
        private readonly HashSet<MazeNode> _visited = new();

        public void Generate(MazeNode[,] grid, Vector2Int size)
        {
            Stack<MazeNode> currentPath = new();

            // get random first node
            Vector2Int currentPosition = new(Random.Range(0, size.x), Random.Range(0, size.y));
            MazeNode currentNode = grid[currentPosition.x, currentPosition.y];

            currentPath.Push(currentNode);

            while (currentPath.Count > 0)
            {
                currentNode = currentPath.Peek();
                _visited.Add(currentNode);

                List<Vector2Int> neighbors = GetAvailableNeighbors(grid, size, currentNode.Position);

                if (neighbors.Count > 0)
                {
                    Vector2Int nextPosition = neighbors[Random.Range(0, neighbors.Count)];
                    MazeNode nextNode = grid[nextPosition.x, nextPosition.y];

                    currentNode.Connect(nextNode);
                    currentPath.Push(nextNode);
                }
                else
                {
                    currentPath.Pop();
                }
            }
        }

        private List<Vector2Int> GetAvailableNeighbors(MazeNode[,] grid, Vector2Int size, Vector2Int position)
        {
            List<Vector2Int> result = new();

            void TryAddNeighbor(int x, int y)
            {
                if (x >= 0 && x < size.x && y >= 0 && y < size.y && !_visited.Contains(grid[x, y]))
                    result.Add(new Vector2Int(x, y));
            }

            TryAddNeighbor(position.x - 1, position.y);
            TryAddNeighbor(position.x + 1, position.y);
            TryAddNeighbor(position.x, position.y - 1);
            TryAddNeighbor(position.x, position.y + 1);

            return result;
        }
    }
}
