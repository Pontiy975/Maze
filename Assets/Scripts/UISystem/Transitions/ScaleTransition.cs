using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace UISystem.Transitions
{
    public class ScaleTransition : ITransition
    {
        private readonly RectTransform _dialogRectTransform;
        private readonly Vector3 _currentScale;
        
        protected readonly float FadeDuration;
        
        public bool InTransition { get; private set; }

        public ScaleTransition(RectTransform dialogRectTransform, float fadeDuration)
        {
            _dialogRectTransform = dialogRectTransform;
            FadeDuration = fadeDuration;

            //float aspect = (float)Screen.width / Screen.height;
            //aspect = Mathf.Clamp(aspect, 0.45f, 0.75f);
            //aspect = 1f - Mathf.InverseLerp(0.45f, 0.75f, aspect);
            //float scaleMultiplier = 0.6f + 0.4f * aspect;

            _currentScale = _dialogRectTransform.localScale;
            //_currentScale = _dialogRectTransform.localScale * scaleMultiplier;
            InTransition = false;
        }
        
        public virtual IEnumerable Show(Action onEnd)
        {
            InTransition = true;
            _dialogRectTransform.DOKill();
            _dialogRectTransform.gameObject.SetActive(true);
            _dialogRectTransform.localScale = Vector3.zero;
            _dialogRectTransform.DOScale(_currentScale, FadeDuration).SetEase(Ease.OutBack).SetUpdate(true);

            yield return new WaitForSecondsRealtime(FadeDuration);
            
            InTransition = false;
            onEnd?.Invoke();
        }

        public virtual IEnumerable Hide(Action onEnd)
        {
            InTransition = true;
            _dialogRectTransform.DOKill();
            _dialogRectTransform.DOScale(Vector3.zero, FadeDuration)
                .SetEase(Ease.InBack)
                .OnComplete(() => _dialogRectTransform.gameObject.SetActive(false))
                .SetUpdate(true);
            
            yield return new WaitForSecondsRealtime(FadeDuration);
            
            onEnd?.Invoke();
            InTransition = false;
        }
    }
}