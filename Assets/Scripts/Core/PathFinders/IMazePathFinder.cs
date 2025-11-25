using System.Collections.Generic;

namespace Maze.Core.PathFinders
{
    public interface IMazePathFinder
    {
        public List<MazeNode> FindPath(MazeNode start, HashSet<MazeNode> exits);
    }
}
