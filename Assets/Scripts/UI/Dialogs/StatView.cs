using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Maze.UI.Dialogs
{
    public class StatView : MonoBehaviour
    {
        private const float SHOW_DURATION = 0.25f;

        [SerializeField] private TMP_Text counter;

        private RectTransform _transform;

        private void Awake()
        {
            _transform = transform as RectTransform;
            _transform.localScale = Vector3.zero;
            counter.text = string.Empty;
        }

        public IEnumerator ShowRoutine(int value)
        {
            _transform.DOKill();
            _transform.localScale = Vector3.zero;
            _transform.DOScale(1f, SHOW_DURATION)
                      .SetEase(Ease.OutBack)
                      .SetUpdate(true);

            float duration = 1f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                float t = elapsed / duration;
                int displayValue = Mathf.FloorToInt(Mathf.Lerp(0, value, t));
                counter.text = displayValue.ToString();

                counter.rectTransform.localScale = Vector3.one + Vector3.one * 0.1f * Mathf.Sin(t * Mathf.PI);

                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }

            counter.text = value.ToString();

            counter.rectTransform.localScale = Vector3.one;
            counter.rectTransform.Bounce(Vector3.one, Vector3.one * 1.3f);
        }
    }
}
