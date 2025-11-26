using Maze.Core;
using Maze.Core.Data;
using Maze.UI.Dialogs;
using Maze.UI.Screens;
using Saves;
using System;
using System.Collections;
using System.Globalization;
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
        [SerializeField] private SaveData saveData;
        [SerializeField] private ResultSaver resultSaver;

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
            saveData.Init();

            _gameScreen = screenManager.GetScreen<GameScreen>();
            _gameScreen.Hide();

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

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
                saveData.SaveAll();
        }

        private void OnApplicationQuit()
        {
            saveData.Reset();
        }

        public void AddNode(MazeNode node)
        {
            _gameScreen?.UpdateDistance(++_traveledDistance);
            CheckExit(node);
        }

        public void ReloadScene()
        {
            saveData.SaveAll();
            SceneManager.LoadScene(0);
        }

        public void StartGame(MazeConfig config)
        {
            _mazeController.Init(config);
            _gameScreen.Show();
            _time = 0;
        }

        private void CheckExit(MazeNode node)
        {
            if (_mazeController.CheckExit(node))
            {
                int resultTime = Mathf.FloorToInt(_time);

                WinDialog dialog = dialogsManager.OpenDialog<WinDialog>();
                dialog.ShowStats(resultTime, _traveledDistance, _mazeController.BestPath.Count);

                resultSaver.AddResult(new()
                {
                    Width = _mazeController.Config.Size.x,
                    Height = _mazeController.Config.Size.y,
                    Exits = _mazeController.Config.Exits,

                    Time = resultTime,
                    Distance = _traveledDistance,
                    BestDistance = _mazeController.BestPath.Count,

                    CompleteTime = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)
                });
            }
        }

    }
}
