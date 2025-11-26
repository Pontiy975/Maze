using UISystem.Transitions;
using System;
using UnityEngine;

namespace UISystem.Screens
{
    public abstract class BaseScreen : SafeArea.SafeArea
    {
        [SerializeField] protected CanvasGroup canvasGroup;
        [SerializeField] protected float transitionDuration = .3f;
        [SerializeField] protected ScreenManager screenManager;

        protected abstract ITransition Transition { get; set; }

        public bool InTransition => Transition.InTransition;

        public virtual void Show(Action onEnd = null)
        {
            SetActive(true);
            if (!InTransition)
                StartCoroutine(Transition.Show(onEnd).GetEnumerator());
        }

        public virtual void Hide(Action onHide = null)
        {
            if (InTransition || !Active) return;
            
            StartCoroutine(Transition.Hide(() =>
            {
                onHide?.Invoke();
                SetActive(false);
            }).GetEnumerator());
        }
    }
}