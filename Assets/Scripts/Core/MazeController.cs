using Maze.Core.Data;
using Maze.Core.Generators;
using Maze.Core.PathFinders;
using Maze.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Maze.Core
{
    public class MazeController : MonoBehaviour
    {
        public event Action OnMazeInitialized;

        [SerializeField] private MazeNode nodePrefab;

        private MazeNode[,] _grid;
        private HashSet<MazeNode> _exits = new();

        #region Properties
        public MazeNode CentralNode { get; private set; }
        public List<MazeNode> BestPath { get; private set; }
        public MazeConfig Config { get; private set; }
        #endregion

        public void Init(MazeConfig config)
        {
            Config = config;
            GenerateMaze();
        }

        public MazeNode GetNode(Vector2Int position) => _grid[position.x, position.y];

        public MazeNode GetNodeAtWorldPosition(Vector2 worldPosition)
        {
            if (_grid == null || Config == null || nodePrefab == null)
                return null;

            float originX = (Config.Size.x - 1) * nodePrefab.Size.x / 2f;
            float originY = (Config.Size.y - 1) * nodePrefab.Size.y / 2f;

            float fx = (worldPosition.x + originX) / nodePrefab.Size.x;
            float fy = (worldPosition.y + originY) / nodePrefab.Size.y;

            int ix = Mathf.RoundToInt(fx);
            int iy = Mathf.RoundToInt(fy);

            if (ix < 0 || ix >= Config.Size.x || iy < 0 || iy >= Config.Size.y)
                return null;

            return _grid[ix, iy];
        }

        public bool TryGetNodeAtWorldPosition(Vector2 worldPosition, out MazeNode node)
        {
            node = GetNodeAtWorldPosition(worldPosition);
            return node != null;
        }

        private void GenerateMaze()
        {
            CreateNodes();

            new DFSMazeGenerator().Generate(_grid, Config.Size);

            MakeExits();
            CentralNode = _grid[Config.Size.x / 2, Config.Size.y / 2];

            BestPath = new BFSPathFinder().FindPath(CentralNode, _exits);

            OnMazeInitialized?.Invoke();
        }

        private void MakeExits()
        {
            int targetCount = Mathf.Min(Config.Exits, 2 * Config.Size.x + 2 * (Config.Size.y - 2));
            while (_exits.Count < targetCount)
            {
                Vector2Int position = Random.Range(0, 4) switch
                {
                    0 => new(0, Random.Range(0, Config.Size.y)),
                    1 => new(Config.Size.x - 1, Random.Range(0, Config.Size.y)),
                    2 => new(Random.Range(0, Config.Size.x), 0),
                    _ => new(Random.Range(0, Config.Size.x), Config.Size.y - 1)
                };

                MazeNode node = _grid[position.x, position.y];

                node.MakeExit(Config.Size);
                _exits.Add(node);
            }
        }

        private void CreateNodes()
        {
            _grid = new MazeNode[Config.Size.x, Config.Size.y];

            for (int x = 0; x < Config.Size.x; x++)
            {
                for (int y = 0; y < Config.Size.y; y++)
                {
                    Vector2 position = GetNodePosition(x, y);

                    _grid[x, y] = Instantiate(nodePrefab, position, Quaternion.identity, transform);
                    _grid[x, y].name += $" [{x},{y}]";

                    _grid[x, y].SetPosition(new(x, y));
                }
            }
        }

        private Vector2 GetNodePosition(int x, int y) => new(x * nodePrefab.Size.x - (Config.Size.x - 1) * nodePrefab.Size.x / 2f,
                                                             y * nodePrefab.Size.y - (Config.Size.y - 1) * nodePrefab.Size.y / 2f);

        #region Save/Load
        public (MazeConfig config, List<TileSaveData>) GetSnapshot()
        {
            List<TileSaveData> tilesData = new();

            for (int x = 0; x < Config.Size.x; x++)
            {
                for (int y = 0; y < Config.Size.y; y++)
                {
                    tilesData.Add(new()
                    {
                        X = x,
                        Y = y,
                        Walls = _grid[x, y].GetWallsSnapshot(),
                        IsExit = _grid[x, y].IsExit,
                        Neighbors = _grid[x, y].GetNeighborsSnapshot(),
                    });
                }
            }

            return (Config, tilesData);
        }

        public void ApplySnapshot(MazeConfig config, List<TileSaveData> tilesData)
        {
            Config = config;
            CreateNodes();

            for (int x = 0; x < Config.Size.x; x++)
            {
                for (int y = 0; y < Config.Size.y; y++)
                {
                    TileSaveData data = tilesData.Find(t => t.X == x && t.Y == y);
                    if (data != null)
                    {
                        MazeNode node = _grid[x, y];
                        node.ApplyWallsSnapshot(data.Walls);

                        HashSet<MazeNode> neighbors = new();
                        foreach (var neighbor in data.Neighbors)
                        {
                            neighbors.Add(_grid[neighbor.x, neighbor.y]);
                        }
                        node.ApplyNeighborsSnapshot(neighbors);
                        
                        if (data.IsExit)
                        {
                            node.MakeExit(Config.Size);
                            _exits.Add(node);
                        }
                    }
                }
            }
            
            CentralNode = _grid[Config.Size.x / 2, Config.Size.y / 2];
            BestPath = new BFSPathFinder().FindPath(CentralNode, _exits);
            Debug.Log(BestPath.Count);
            OnMazeInitialized?.Invoke();
        }
        #endregion
    }
}