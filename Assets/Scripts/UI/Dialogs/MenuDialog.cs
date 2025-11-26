using System.Collections;
using System.Collections.Generic;
using UISystem.Dialogs;
using UISystem.Transitions;
using UnityEngine;
using UnityEngine.UI;

namespace Maze.UI.Dialogs
{
    public class MenuDialog : SimpleDialogWithTransition
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button loadButton;
        [SerializeField] private Button resultsButton;

        protected override ITransition Transition { get; set; }

        protected override void Init()
        {
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
        }

        private void OnDestroy()
        {
            playButton.onClick.RemoveAllListeners();
            resultsButton.onClick.RemoveAllListeners();
        }
    }
}
