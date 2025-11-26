using TMPro;
using UISystem.Dialogs;
using UISystem.Screens;
using UISystem.Transitions;
using UnityEngine;

namespace Maze
{
    public class GameScreen : BaseScreen
    {
        [SerializeField] private DialogsManager dialogsManager;
        [SerializeField] private TMP_Text distanceText;

        protected override ITransition Transition { get; set; }

        protected override void Awake()
        {
            base.Awake();

            Transition = new FadeTransition(canvasGroup, transitionDuration);
            screenManager.RegisterScreen(this, true);
        }

        public void UpdateDistance(int distance)
        {
            distanceText.text = $"TraveledDistance: {distance}";
        }
    }
}
