using UnityEngine;

namespace Maze.Core.Data
{
    public class MazeConfig
    {
        public Vector2Int Size { get; private set; }
        public int Exits { get; private set; }

        public MazeConfig(Vector2Int size, int exits)
        {
            Size = size;
            Exits = exits;
        }
    }

    public static class MazeConfigFactory
    {
        public static MazeConfig Create(Vector2Int size, int exits) => new(size, exits);
    }
}
