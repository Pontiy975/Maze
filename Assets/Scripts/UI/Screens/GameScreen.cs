using UISystem.Dialogs;
using UISystem.Screens;
using UISystem.Transitions;
using UnityEngine;
using Utils;

namespace Maze.UI.Screens
{
    public class GameScreen : BaseScreen
    {
        [SerializeField] private DialogsManager dialogsManager;
        [field: SerializeField] public CustomJoystick Joystick { get; private set; }
        [SerializeField] private SerializableDictionaryBase<CounterType, Counter> counters;
        protected override ITransition Transition { get; set; }

        protected override void Awake()
        {
            base.Awake();

            Transition = new FadeTransition(canvasGroup, transitionDuration);
            screenManager.RegisterScreen(this, true);

            UpdateDistance(0);
            UpdateTime(0);
        }

        public void UpdateDistance(int value) => counters[CounterType.Distance].SetValue(value);

        public void UpdateTime(int value) => counters[CounterType.Time].SetValue(value);
    }
}
