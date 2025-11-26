using Maze.Game;
using UISystem.Dialogs;
using UISystem.Transitions;
using UnityEngine;
using UnityEngine.UI;

namespace Maze.UI.Dialogs
{
    public class ResultsDialog : SimpleDialogWithTransition
    {
        [SerializeField] private ResultSaver resultSaver;
        [SerializeField] private ResultRow rowPrefab;
        [SerializeField] private RectTransform container;
        [SerializeField] private Button backButton;

        [SerializeField] private Color[] colors;

        protected override ITransition Transition { get; set; }

        protected override void Init()
        {
            if (resultSaver.Initialized)
                InitRows();
            else
                resultSaver.OnInitialized += OnResultSaverInitialized;


            backButton.onClick.AddListener(() =>
            {
                Hide();
                dialogsManager.OpenDialog<MenuDialog>();
            });
        }

        private void OnDestroy()
        {
            backButton.onClick.RemoveAllListeners();
        }

        private void OnResultSaverInitialized()
        {
            resultSaver.OnInitialized -= OnResultSaverInitialized;
            InitRows();
        }

        private void InitRows()
        {
            for (int i = 0; i < resultSaver.Results.Count; i++)
            {
                ResultEntry result = resultSaver.Results[i];
                ResultRow row = Instantiate(rowPrefab, container);
                row.SetData(i + 1, (result.Width, result.Height), result.Time, result.Distance, result.BestDistance, colors[i % 2]);
            }
        }
    }
}
