using Maze.Core;
using Maze.Core.Data;
using Maze.UI.Dialogs;
using Maze.UI.Screens;
using System.Collections;
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

        [field: SerializeField] public MazeModel MazeModel { get; private set; }

        [Inject] private MazeController _mazeController;

        private int _traveledDistance = 0;
        private float _time;

        private GameScreen _gameScreen;

        private void Awake()
        {
            Application.targetFrameRate = 60;
            Time.timeScale = 1f;
        }

        private IEnumerator Start()
        {
            _gameScreen = screenManager.GetScreen<GameScreen>();
            _gameScreen.Hide();

            _time = 0;

            yield return null;
            dialogsManager.OpenDialog<SettingsDialog>();
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

        public void InitMaze(MazeConfig config)
        {
            _mazeController.Init(config);
            _gameScreen.Show();
        }
    }
}
