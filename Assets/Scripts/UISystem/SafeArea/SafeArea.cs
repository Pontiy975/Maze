using UnityEngine;

namespace UISystem.SafeArea
{
    public class SafeArea : MonoBehaviour
    {
        private RectTransform _panel;
        private GameObject _gameObject;

        private bool _isInitialized = false;
        
        protected virtual void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            if(_isInitialized)
                return;

            _isInitialized = true;
            
            _panel = GetComponent<RectTransform>();
            _gameObject = gameObject;
            Rect safeAreRect = Screen.safeArea;
            
            // if(safeAreRect.position != Vector2.zero || (int)safeAreRect.width != Screen.width 
            //                                         || (int)safeAreRect.height != Screen.height)
            if(safeAreRect.position.y > 40)
                ApplySafeArea(safeAreRect);
        }

        public void SetActive(bool value)
        {
            Initialize();
            
            if(_gameObject)
                _gameObject.SetActive(value);
        }

        protected virtual void ApplySafeArea(Rect safeAreRect)
        {
            // Convert safe area rectangle from absolute pixels to normalised anchor coordinates
            Vector2 anchorMin = safeAreRect.position;
            Vector2 anchorMax = safeAreRect.position + safeAreRect.size;
            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;
            _panel.anchorMin = anchorMin;
            _panel.anchorMax = anchorMax;
        }
        
        public bool Active => _gameObject.activeSelf;
        protected RectTransform Panel => _panel;
    }
}