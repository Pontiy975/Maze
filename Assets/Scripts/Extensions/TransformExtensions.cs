using DG.Tweening;
using System;
using UnityEngine;

public static class TransformExtensions
{
    public static float GetSqrDistance(this Transform origin, Transform target)
    {
        return (target.position - origin.position).sqrMagnitude;
    }

    public static float GetSqrDistance(this Transform origin, Vector3 target)
    {
        return (target - origin.position).sqrMagnitude;
    }
    
    public static bool IsSameTransform(this Transform origin, Transform target)
    {
        return origin == target;
    }

    public static Vector3 GetDirectionToPoint(this Transform origin, Transform target, Space space = Space.World)
    {
        return space == Space.World ? (target.position - origin.position).normalized : (target.localPosition - origin.localPosition).normalized;
    }

    public static Rect GetWorldRect(this RectTransform transform)
    {
        Vector3[] corners = new Vector3[4];
        transform.GetWorldCorners(corners);

        return new(corners[0].x, corners[0].y,
                   corners[2].x - corners[0].x,
                   corners[2].y - corners[0].y);
    }

    public static void Bounce(this Transform transform, Vector3 baseScale, Vector3 targetScale, float duration = 0.2f, int loops = 0, Action onComplete = null)
    {
        transform.DOKill();
        transform.localScale = baseScale;

        transform.DOScale(targetScale, duration)
                 .SetEase(Ease.OutSine)
                 .SetLoops(loops, LoopType.Yoyo)
                 .SetUpdate(true)
                 .OnComplete(() =>
                 {
                     if (loops == 0)
                     {
                         transform.DOScale(baseScale, duration / 2f)
                                  .SetEase(Ease.InSine)
                                  .SetUpdate(true)
                                  .OnComplete(() => onComplete?.Invoke());
                     }
                     else
                     {
                         onComplete?.Invoke();
                     }
                 });
    }

    public static void ResetBounce(this Transform transform, Vector3 baseScale)
    {
        transform.DOKill();
        transform.localScale = baseScale;
    }
}
