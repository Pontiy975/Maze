using Maze.Core;
using System.Collections.Generic;
using UISystem.Screens;
using UnityEngine;

namespace Maze.Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private ScreenManager screenManager;

        private HashSet<MazeNode> _visitedNodes = new();
        private int _traveledDistance = 0;

        private GameScreen _gameScreen;

        private void Start()
        {
            _gameScreen = screenManager.GetScreen<GameScreen>();
        }

        public void AddNode(MazeNode node)
        {
            _visitedNodes.Add(node);
            _traveledDistance++;

            _gameScreen?.UpdateDistance(_traveledDistance);
        }
    }
}
