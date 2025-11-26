using Maze.Core;
using Maze.UI.Dialogs;
using Maze.UI.Screens;
using System.Collections.Generic;
using UISystem.Dialogs;
using UISystem.Screens;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Maze.Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private ScreenManager screenManager;
        [SerializeField] private DialogsManager dialogsManager;

        [Inject] private MazeController _mazeController;

        private List<MazeNode> _visitedNodes = new();
        private int _traveledDistance = 0;

        private GameScreen _gameScreen;
        private float _time;

        private void Awake()
        {
            Application.targetFrameRate = 60;
            Time.timeScale = 1f;
        }

        private void Start()
        {
            _gameScreen = screenManager.GetScreen<GameScreen>();
            _time = 0;
        }

        private void Update()
        {
            _time += Time.deltaTime;
            _gameScreen?.UpdateTime(Mathf.FloorToInt(_time));
        }

        private void OnDestroy()
        {
            screenManager.ClearScreens();
            dialogsManager.ClearDialogs();
        }

        public void AddNode(MazeNode node)
        {
            _visitedNodes.Add(node);
            
            _gameScreen?.UpdateDistance(++_traveledDistance);

            if (_mazeController.CheckExit(node))
            {
                WinDialog dialog = dialogsManager.OpenDialog<WinDialog>();
                dialog.ShowStats(Mathf.FloorToInt(_time), _traveledDistance, _mazeController.BestPath.Count);
            }
        }

        public void ReloadScene()
        {
            SceneManager.LoadScene(0);
        }
    }
}
