using Maze.Core.Data;
using Maze.Core.Generators;
using Maze.Core.PathFinders;
using System.Collections.Generic;
using UnityEngine;

namespace Maze.Core
{
    public class MazeController : MonoBehaviour
    {
        [SerializeField] private MazeModel model;
        [SerializeField] private MazeNode nodePrefab;

        private MazeNode[,] _grid;
        private HashSet<MazeNode> _exits = new();

        #region Properties
        public MazeNode CentralNode { get; private set; }
        public List<MazeNode> BestPath { get; private set; }
        #endregion

        private void Awake()
        {
            GenerateMaze();
        }

        public MazeNode GetNodeAtWorldPosition(Vector2 worldPosition)
        {
            if (_grid == null || model == null || nodePrefab == null)
                return null;

            float originX = (model.Size.x - 1) * nodePrefab.Size.x / 2f;
            float originY = (model.Size.y - 1) * nodePrefab.Size.y / 2f;

            float fx = (worldPosition.x + originX) / nodePrefab.Size.x;
            float fy = (worldPosition.y + originY) / nodePrefab.Size.y;

            int ix = Mathf.RoundToInt(fx);
            int iy = Mathf.RoundToInt(fy);

            if (ix < 0 || ix >= model.Size.x || iy < 0 || iy >= model.Size.y)
                return null;

            return _grid[ix, iy];
        }

        public bool TryGetNodeAtWorldPosition(Vector2 worldPosition, out MazeNode node)
        {
            node = GetNodeAtWorldPosition(worldPosition);
            return node != null;
        }

        public bool CheckExit(MazeNode node) => _exits.Contains(node);

        private void GenerateMaze()
        {
            CreateNodes();
            new DFSMazeGenerator().Generate(_grid, model.Size);
            MakeExits();
            CentralNode = _grid[model.Size.x / 2, model.Size.y / 2];

            BestPath = new BFSPathFinder().FindPath(CentralNode, _exits);
        }

        private void MakeExits()
        {
            int targetCount = Mathf.Min(model.ExitsCount, 2 * model.Size.x + 2 * (model.Size.y - 2));
            while (_exits.Count < targetCount)
            {
                Vector2Int position = Random.Range(0, 4) switch
                {
                    0 => new(0, Random.Range(0, model.Size.y)),
                    1 => new(model.Size.x - 1, Random.Range(0, model.Size.y)),
                    2 => new(Random.Range(0, model.Size.x), 0),
                    _ => new(Random.Range(0, model.Size.x), model.Size.y - 1)
                };

                MazeNode node = _grid[position.x, position.y];

                node.MakeExit(model.Size);
                _exits.Add(node);
            }
        }

        private void CreateNodes()
        {
            _grid = new MazeNode[model.Size.x, model.Size.y];

            for (int x = 0; x < model.Size.x; x++)
            {
                for (int y = 0; y < model.Size.y; y++)
                {
                    Vector2 position = GetNodePosition(x, y);

                    _grid[x, y] = Instantiate(nodePrefab, position, Quaternion.identity, transform);
                    _grid[x, y].name += $" [{x},{y}]";

                    _grid[x, y].SetPosition(new Vector2Int(x, y));
                }
            }
        }

        private Vector2 GetNodePosition(int x, int y) => new(x * nodePrefab.Size.x - (model.Size.x - 1) * nodePrefab.Size.x / 2f,
                                                             y * nodePrefab.Size.y - (model.Size.y - 1) * nodePrefab.Size.y / 2f);
    }
}
