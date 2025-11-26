using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UISystem.Transitions
{
    public class SimpleDialogTransition : ScaleTransition
    {
        private readonly Image _background;
        private readonly Color _backgroundColor;

        public SimpleDialogTransition(RectTransform dialogRectTransform, Image background, float fadeDuration)
            : base(dialogRectTransform, fadeDuration)
        {
            if (background)
            {
                _background = background;
                _backgroundColor = background.color;
            }
        }

        public override IEnumerable Show(Action onEnd)
        {
            _background?.TweenAlpha(0, _backgroundColor.a, FadeDuration, Ease.OutSine);

            
            yield return base.Show(onEnd).GetEnumerator();
        }

        public override IEnumerable Hide(Action onEnd)
        {
            _background?.TweenAlpha(_backgroundColor.a, 0, FadeDuration, Ease.InSine, true);

            
            yield return base.Hide(onEnd).GetEnumerator();
        }
    }
}