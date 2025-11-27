using Maze.Core.Data;
using Maze.Game;
using System.Collections.Generic;

namespace Maze.Core
{
    public static class MazeSaveService
    {
        public static (MazeConfig config, List<TileSaveData>) GetSnapshot(MazeConfig config, MazeNode[,] grid)
        {
            List<TileSaveData> tilesData = new();

            for (int x = 0; x < config.Size.x; x++)
            {
                for (int y = 0; y < config.Size.y; y++)
                {
                    tilesData.Add(new()
                    {
                        X = x,
                        Y = y,
                        Walls = grid[x, y].GetWallsSnapshot(),
                        IsExit = grid[x, y].IsExit,
                        Neighbors = grid[x, y].GetNeighborsSnapshot(),
                    });
                }
            }

            return (config, tilesData);
        }

        public static void ApplySnapshot(MazeNode[,] grid, MazeConfig config, List<TileSaveData> tilesData)
        {
            for (int x = 0; x < config.Size.x; x++)
            {
                for (int y = 0; y < config.Size.y; y++)
                {
                    TileSaveData data = tilesData.Find(t => t.X == x && t.Y == y);
                    if (data != null)
                    {
                        MazeNode node = grid[x, y];
                        node.ApplyWallsSnapshot(data.Walls);

                        HashSet<MazeNode> neighbors = new();
                        foreach (var neighbor in data.Neighbors)
                        {
                            neighbors.Add(grid[neighbor.x, neighbor.y]);
                        }
                        node.ApplyNeighborsSnapshot(neighbors);

                        if (data.IsExit)
                            node.MakeExit(config.Size);
                    }
                }
            }
        }
    }
}
