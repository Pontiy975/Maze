using UnityEngine;

namespace UISystem.SafeArea
{
    public class TopSafeAreaOffset : SafeArea
    {
        [SerializeField] private RectTransform safeAreaOffsetTransform;
        [SerializeField] private float safeAreaOffset = 25;
        
        protected override void ApplySafeArea(Rect safeAreRect)
        {
            base.ApplySafeArea(safeAreRect);
            safeAreaOffsetTransform.anchoredPosition += new Vector2(0, safeAreaOffset);
        }
    }
}