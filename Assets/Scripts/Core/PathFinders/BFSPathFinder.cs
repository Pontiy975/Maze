using System.Collections.Generic;

namespace Maze.Core.PathFinders
{
    public class BFSPathFinder : IMazePathFinder
    {
        public List<MazeNode> FindPath(MazeNode start, HashSet<MazeNode> exits)
        {
            Queue<MazeNode> queue = new();
            HashSet<MazeNode> visited = new();
        
            // key - child, value - parent
            Dictionary<MazeNode, MazeNode> parents = new();

            queue.Enqueue(start);
            visited.Add(start);

            while (queue.Count > 0)
            {
                MazeNode current = queue.Dequeue();

                if (exits.Contains(current))
                    return GetPath(parents, current);

                foreach (MazeNode neighbor in current.Neighbors)
                {
                    if (!visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        parents[neighbor] = current;
                        queue.Enqueue(neighbor);
                    }
                }
            }

            return new();
        }

        private List<MazeNode> GetPath(Dictionary<MazeNode, MazeNode> parents, MazeNode endNode)
        {
            List<MazeNode> path = new();
            MazeNode current = endNode;

            while (parents.ContainsKey(current))
            {
                path.Add(current);
                current = parents[current];
            }

            path.Add(current);
            path.Reverse();

            return path;
        }
    }
}
