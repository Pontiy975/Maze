using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class TweenExtensions
{
    public static void TweenAlpha(this Image image, float from, float to, float duration = .3f, Ease ease = Ease.Linear, 
        bool turnOffInEnd = false, bool unscaled = true)
    {
        if(!image.gameObject.activeSelf)
            image.gameObject.SetActive(true);

        image.SetAlpha(from);
           
        image.DOFade(to, duration)
            .SetEase(ease)
            .OnComplete(() =>
            {
                if(turnOffInEnd)
                    image.gameObject.SetActive(false);
            })
            .SetUpdate(unscaled);
    }
    public static void TweenAlpha(this TMP_Text text, float from, float to, float duration = .3f, Ease ease = Ease.Linear,
        bool turnOffInEnd = false, bool unscaled = true)
    {
        if (!text.gameObject.activeSelf)
            text.gameObject.SetActive(true);

        text.SetAlpha(from);

        text.DOFade(to, duration)
            .SetEase(ease)
            .OnComplete(() =>
            {
                if (turnOffInEnd)
                    text.gameObject.SetActive(false);
            })
            .SetUpdate(unscaled);
    }

    public static void TweenAlpha(this CanvasGroup canvasGroup, float from, float to, float duration = .3f, Ease ease = Ease.Linear, 
        bool turnOffInEnd = false, bool unscaled = true)
    {
        if(!canvasGroup.gameObject.activeSelf)
            canvasGroup.gameObject.SetActive(true);

        canvasGroup.alpha = from;
           
        canvasGroup.DOFade(to, duration)
            .SetEase(ease)
            .OnComplete(() =>
            {
                if(turnOffInEnd)
                    canvasGroup.gameObject.SetActive(false);
            })
            .SetUpdate(unscaled);
    }

    public static void SetAlpha(this Image image, float newAlpha)
    {
        Color color = image.color;
        color.a = newAlpha;
        image.color = color;
    }

    public static void SetAlpha(this TMP_Text text, float newAlpha)
    {
        Color color = text.color;
        color.a = newAlpha;
        text.color = color;
    }

    public static Sequence ClearSequence(this Sequence sequence)
    {
        if (sequence.IsActive() && (!sequence.IsComplete() || sequence.IsPlaying()))
            sequence.Kill();

        if (!sequence.IsActive())
            sequence = DOTween.Sequence();

        return sequence;
    }
}