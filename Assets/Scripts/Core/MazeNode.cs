using Maze.Game;
using System.Collections.Generic;
using UnityEngine;

namespace Maze.Core
{
    public enum NodeState
    {
        Available,
        Visited,
        Player
    }

    public class MazeNode : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer sprite;

        /// <summary>
        /// 0 - top,
        /// 1 - right,
        /// 2 - bottom,
        /// 3 - left
        /// </summary>
        [SerializeField] private GameObject[] walls;

        private HashSet<MazeNode> _neighbors = new();

        #region Properties
        public Vector2 Size => sprite.bounds.size;
        public Vector2Int Position { get; private set; }
        public bool IsExit { get; private set; }
        public IReadOnlyCollection<MazeNode> Neighbors => _neighbors;
        public NodeState State { get; private set; }
        #endregion

        public void SetPosition(Vector2Int position) => Position = position;
        public void SetState(NodeState state)
        {
            if (state == NodeState.Player)
                sprite.color = Color.red;
            State = state;
        }

        public void Connect(MazeNode node)
        {
            _neighbors.Add(node);
            node._neighbors.Add(this);

            Vector2Int direction = node.Position - Position;

            if (direction.x != 0)
            {
                walls[direction.x > 0 ? 1 : 3].SetActive(false);
                node.walls[direction.x > 0 ? 3 : 1].SetActive(false);
            }
            else
            {
                walls[direction.y > 0 ? 0 : 2].SetActive(false);
                node.walls[direction.y > 0 ? 2 : 0].SetActive(false);
            }
        }

        public void MakeExit(Vector2Int size)
        {
            IsExit = true;

            if (Position.x == 0)
                walls[3].SetActive(false);
            else if (Position.x == size.x - 1)
                walls[1].SetActive(false);
            else if (Position.y == 0)
                walls[2].SetActive(false);
            else
                walls[0].SetActive(false);
        }

        #region Save/Load
        public byte GetWallsSnapshot()
        {
            byte snapshot = 0;

            if (walls[0].activeInHierarchy) snapshot |= (byte)WallFlags.Up;
            if (walls[1].activeInHierarchy) snapshot |= (byte)WallFlags.Right;
            if (walls[2].activeInHierarchy) snapshot |= (byte)WallFlags.Down;
            if (walls[3].activeInHierarchy) snapshot |= (byte)WallFlags.Left;

            return snapshot;
        }

        public void ApplyWallsSnapshot(byte snapshot)
        {
            walls[0].SetActive((snapshot & (byte)WallFlags.Up) != 0);
            walls[1].SetActive((snapshot & (byte)WallFlags.Right) != 0);
            walls[2].SetActive((snapshot & (byte)WallFlags.Down) != 0);
            walls[3].SetActive((snapshot & (byte)WallFlags.Left) != 0);
        }

        public List<(int, int)> GetNeighborsSnapshot()
        {
            List<(int x, int y)> list = new();

            foreach (var neighbor in _neighbors)
                list.Add((neighbor.Position.x, neighbor.Position.y));

            return list;
        }

        public void ApplyNeighborsSnapshot(HashSet<MazeNode> neighbors)
        {
            _neighbors = neighbors;
        }

        public bool HasWall(WallFlags flag) => (GetWallsSnapshot() & (byte)flag) != 0;
        #endregion
    }
}
