using System;
using System.Collections;
using UnityEngine;

namespace UISystem.Transitions
{
    public class FadeTransition : ITransition
    {
        private readonly CanvasGroup _canvasGroup;
        private readonly float _animationDuration;

        public bool InTransition { get; private set; }

        public FadeTransition(CanvasGroup canvasGroup, float animationDuration)
        {
            _canvasGroup = canvasGroup;
            _animationDuration = animationDuration;
            InTransition = false;
        }

        public virtual IEnumerable Show(Action onEnd)
        {
            if(!InTransition)
                yield return FadeCoroutine(0, 1, () =>
                {
                    _canvasGroup.interactable = true;
                    _canvasGroup.blocksRaycasts = true;
                    onEnd?.Invoke();
                });
        }

        public virtual IEnumerable Hide(Action onEnd)
        {
            if (InTransition) 
                yield break;
            
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;

            yield return FadeCoroutine(1, 0);
            onEnd?.Invoke();
        }
        
        private IEnumerator FadeCoroutine(float from, float to, Action endAction = null)
        {
            _canvasGroup.alpha = from;
            float timer = 0;
            float delta = to - from;
            InTransition = true;
            
            while (timer <= _animationDuration)
            {
                _canvasGroup.alpha = from + delta * timer / _animationDuration;
                yield return null;
                timer += Time.unscaledDeltaTime;
            }

            _canvasGroup.alpha = to;
            InTransition = false;
            endAction?.Invoke();
        }
    }
}