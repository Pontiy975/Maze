using Maze.Game;
using UISystem.Dialogs;
using UISystem.Transitions;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Maze.UI.Dialogs
{
    public class MenuDialog : SimpleDialogWithTransition
    {
        [SerializeField] private SessionSaver sessionSaver;
        [SerializeField] private Button playButton;
        [SerializeField] private Button loadButton;
        [SerializeField] private Button resultsButton;

        [Inject] private GameManager _gameManager;

        protected override ITransition Transition { get; set; }

        protected override void Init()
        {
            if (sessionSaver.Initialized)
                loadButton.interactable = sessionSaver.SessionSaveData != null;
            else
                sessionSaver.OnInitialized += OnSessionInitialized;

            playButton.onClick.AddListener(() =>
            {
                Hide();
                dialogsManager.OpenDialog<SettingsDialog>();
            });

            resultsButton.onClick.AddListener(() =>
            {
                Hide();
                dialogsManager.OpenDialog<ResultsDialog>();
            });

            loadButton.onClick.AddListener(() =>
            {
                Hide();
                _gameManager.LoadSession();
            });
        }

        private void OnDestroy()
        {
            playButton.onClick.RemoveAllListeners();
            resultsButton.onClick.RemoveAllListeners();
        }

        private void OnSessionInitialized()
        {
            sessionSaver.OnInitialized -= OnSessionInitialized;
            loadButton.interactable = sessionSaver.SessionSaveData != null;
        }
    }
}
