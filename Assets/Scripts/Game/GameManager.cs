using Maze.Core;
using Maze.UI.Screens;
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
        private float _time;

        private void Start()
        {
            _gameScreen = screenManager.GetScreen<GameScreen>();
            _time = 0;
        }

        private void Update()
        {
            _time += Time.deltaTime;
            _gameScreen?.UpdateTime((int)_time);
        }

        public void AddNode(MazeNode node)
        {
            _visitedNodes.Add(node);
            _traveledDistance++;

            _gameScreen?.UpdateDistance(_traveledDistance);
        }
    }
}
