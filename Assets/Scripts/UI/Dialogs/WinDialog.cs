using DG.Tweening;
using Maze.Game;
using System.Collections;
using UISystem.Dialogs;
using UISystem.Transitions;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Maze.UI.Dialogs
{
    public class WinDialog : SimpleDialogWithTransition
    {
        [SerializeField] private StatView timeStat;
        [SerializeField] private StatView distanceStat;
        [SerializeField] private StatView bestDisanceStat;
        [SerializeField] private Button playButton;

        [Inject] private GameManager _gameManager;

        protected override ITransition Transition { get; set; }

        protected override void Init()
        {
            playButton.transform.localScale = Vector3.zero;
            playButton.onClick.AddListener(_gameManager.ReloadScene);
        }

        private void OnDestroy()
        {
            playButton.onClick.RemoveAllListeners();
        }

        public void ShowStats(int time, int distance, int best)
        {
            StartCoroutine(ShowStatsRoutine(time, distance, best));
        }

        private IEnumerator ShowStatsRoutine(int time, int distance, int best)
        {
            yield return StartCoroutine(timeStat.ShowRoutine(time));
            yield return StartCoroutine(distanceStat.ShowRoutine(distance));
            yield return StartCoroutine(bestDisanceStat.ShowRoutine(best));
            
            yield return new WaitForSecondsRealtime(0.2f);
            
            playButton.transform.DOScale(Vector2.one, 0.3f)
                                .SetEase(Ease.OutBack)
                                .SetUpdate(true);
        }
    }
}
