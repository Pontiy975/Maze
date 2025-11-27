using Maze.Core.Data;
using Maze.Core.Generators;
using Maze.Core.PathFinders;
using Maze.Game;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Maze.Core
{
    public class MazeController : MonoBehaviour
    {
        public event Action OnMazeInitialized;

        [SerializeField] private MazeNode nodePrefab;

        [Inject] private IMazeGenerator _generator;

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

            _generator.Generate(_grid, Config.Size);

            MakeExits();
            CalculateBestPath();

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

        private void CalculateBestPath()
        {
            CentralNode = _grid[Config.Size.x / 2, Config.Size.y / 2];
            BestPath = new BFSPathFinder().FindPath(CentralNode, _exits);
        }

        #region Save/Load
        public (MazeConfig config, List<TileSaveData>) GetSnapshot() => MazeSaveService.GetSnapshot(Config, _grid);

        public void ApplySnapshot(SessionSaveData snapshot)
        {
            Config = MazeConfigFactory.Create(new(snapshot.Width, snapshot.Height), snapshot.Exits);

            CreateNodes();

            MazeSaveService.ApplySnapshot(_grid, Config, snapshot.Tiles);

            _exits.Clear();
            for (int x = 0; x < Config.Size.x; x++)
            {
                for (int y = 0; y < Config.Size.y; y++)
                {
                    if (_grid[x, y].IsExit)
                        _exits.Add(_grid[x, y]);
                }
            }

            CalculateBestPath();
            OnMazeInitialized?.Invoke();
        }
        #endregion
    }
}