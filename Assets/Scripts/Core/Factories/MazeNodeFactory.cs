using UnityEngine;
using Zenject;

namespace Maze.Core.Factories
{
    public class MazeNodeFactory : IMazeNodeFactory
    {
        private readonly MazeNode _nodePrefab;

        [Inject]
        public MazeNodeFactory(MazeNode prefab)
        {
            _nodePrefab = prefab;
        }

        public MazeNode Create(Vector2Int gridSize, Vector2Int gridPosition, Transform parent)
        {
            Vector2 position = CalculateWorldPosition(gridSize, gridPosition);
            MazeNode node = Object.Instantiate(_nodePrefab, position, Quaternion.identity, parent);
            node.name += $" [{gridPosition.x},{gridPosition.y}]";
            node.SetPosition(gridPosition);

            return node;
        }

        private Vector2 CalculateWorldPosition(Vector2Int gridSize, Vector2Int gridPos)
        {
            float width = _nodePrefab.Size.x;
            float height = _nodePrefab.Size.y;

            float offsetX = (gridSize.x - 1) * width * 0.5f;
            float offsetY = (gridSize.y - 1) * height * 0.5f;

            return new Vector2(
                gridPos.x * width - offsetX,
                gridPos.y * height - offsetY
            );
        }
    }
}
