using Maze.Core;
using Maze.Core.Data;
using Maze.Core.Generators;
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
        public static Func<Vector2Int> GetPlayerPosition;
        public static Action<Vector2Int> OnPlayerLoaded;

        [SerializeField] private GameStateMachine gameStateMachine;
        [SerializeField] private ScreenManager screenManager;
        [SerializeField] private DialogsManager dialogsManager;
        [SerializeField] private SaveData saveData;
        [SerializeField] private ResultSaver resultSaver;
        [SerializeField] private SessionSaver sessionSaver;
        [SerializeField] private ParticleSystem confettiFX;

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
            gameStateMachine.SetState(GameState.Menu);

            _gameScreen = screenManager.GetScreen<GameScreen>();
            _gameScreen.Hide();

            yield return null;
            dialogsManager.OpenDialog<MenuDialog>();
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
            {
                SaveSession();
                saveData.SaveAll();
            }
        }

        private void OnApplicationQuit()
        {
            SaveSession();
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
            gameStateMachine.SetState(GameState.InGame);
            _gameScreen.Show();

            _mazeController.Init(config);
            _time = 0;
        }

        private void CheckExit(MazeNode node)
        {
            if (node.IsExit)
                StartCoroutine(WinRoutine(node));
        }

        private IEnumerator WinRoutine(MazeNode node)
        {
            Instantiate(confettiFX, node.transform.position, Quaternion.identity);

            gameStateMachine.SetState(GameState.Menu);
            yield return new WaitForSeconds(2f);

            int resultTime = Mathf.FloorToInt(_time);

            WinDialog dialog = dialogsManager.OpenDialog<WinDialog>();
            dialog.ShowStats(resultTime, _traveledDistance, _mazeController.BestPath.Count);
            sessionSaver.Clear();

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

        private void SaveSession()
        {
            if (gameStateMachine.CheckStates(GameState.InGame))
                sessionSaver.SaveSession(_mazeController.GetSnapshot(), _traveledDistance, Mathf.FloorToInt(_time), GetPlayerPosition.Invoke());
        }

        public void LoadSession()
        {
            gameStateMachine.SetState(GameState.InGame);
            _gameScreen?.Show();

            SessionSaveData snapshot = sessionSaver.SessionSaveData;

            _mazeController.ApplySnapshot(snapshot);
            OnPlayerLoaded?.Invoke(new(snapshot.PlayerX, snapshot.PlayerY));

            _time = snapshot.Time;

            _traveledDistance = snapshot.Distance;
            _gameScreen?.UpdateDistance(_traveledDistance);
        }
    }
}
