using UISystem.Screens;
using System;
using System.Collections;
using UnityEngine;

namespace UISystem.Transitions
{
    public class SlideTransition : ITransition
    {
        public bool InTransition { get; private set; }

        private SlideData _slideData;
        private readonly float _animationDuration;
        private readonly RectTransform _transform;
        private readonly ScreenManager _screenManager;

        public SlideTransition(RectTransform transform, SlideData slideData, ScreenManager screenManager, float animationDuration)
        {
            _transform = transform;
            _slideData = slideData;
            _animationDuration = animationDuration;
            InTransition = false;
            _screenManager = screenManager;
        }

        public IEnumerable Show(Action onEnd)
        {
            if (!InTransition)
            {
                Vector2 from = Vector2.zero;
                Vector2 to = Vector2.zero;

                from.x = _screenManager.CanvasTransform.sizeDelta.x * (_slideData.Direction > 0 ? -1 : 1);
                _transform.anchoredPosition = from;

                yield return SlideRoutine(from, to, onEnd);
            }
        }

        public IEnumerable Hide(Action onEnd)
        {
            if (InTransition)
                yield break;

            Vector2 from = Vector2.zero;
            Vector2 to = Vector2.zero;

            to.x = _screenManager.CanvasTransform.sizeDelta.x * (_slideData.Direction > 0 ? 1 : -1);

            yield return SlideRoutine(from, to, onEnd);
        }

        private IEnumerator SlideRoutine(Vector2 from, Vector2 to, Action endAction = null)
        {
            float timer = 0;
            Vector2 delta = to - from;
            InTransition = true;

            while (timer <= _animationDuration)
            {
                _transform.anchoredPosition = from + delta * timer / _animationDuration;
                yield return null;
                timer += Time.unscaledDeltaTime;
            }

            _transform.anchoredPosition = to;
            InTransition = false;
            endAction?.Invoke();
        }

        public class SlideData
        {
            public int Direction { get; set; } = 1;
        }
    }
}
