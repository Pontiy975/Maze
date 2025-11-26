using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace UISystem.Screens
{
    [CreateAssetMenu(fileName = "ScreenManager", menuName = "ScriptableObjects/UI/ScreenManager")]
    public class ScreenManager : ScriptableObject
    {
        public event Action<BaseScreen> OnScreenChange;

        public RectTransform CanvasTransform { get; private set; }
        public BaseScreen CurrentScreen => _currentScreen;
        
        private BaseScreen _currentScreen;
        private readonly Dictionary<Type, BaseScreen> _screens = new();

        public T GetScreen<T>() where T : BaseScreen
        {
            if (_screens.ContainsKey(typeof(T)))
                return _screens[typeof(T)] as T;
            
            return null;
        }

        public void RegisterScreen(BaseScreen screen, bool isCurrentScreen = false)
        {
            if(!_screens.ContainsKey(screen.GetType()))
                _screens.Add(screen.GetType(), screen);

            if (isCurrentScreen)
            {
                _currentScreen = screen;
                CanvasTransform = (RectTransform)_currentScreen.transform.parent;
            }
            
            screen.SetActive(isCurrentScreen);
        }

        public T ChangeScreen<T>(Action<T> onShowEnd = null) where T : BaseScreen
        {
            if (!_screens.ContainsKey(typeof(T)) || _currentScreen == null || _currentScreen.InTransition)
                return null;

            if (_currentScreen.Active)
            {
                _currentScreen.Hide();
                OnScreenChange?.Invoke(_currentScreen);
            }
            
            _currentScreen = _screens[typeof(T)];
            _currentScreen.Show(() => onShowEnd?.Invoke(_currentScreen as T));
            return _currentScreen as T;
        }

        public void ClearScreens()
        {
            _screens.Clear();
        }
        
        public Vector2 WorldToCanvasPosition(Vector3 worldPosition, RectTransform rectTransform = null)
        {
            if (rectTransform == null)
                rectTransform = CanvasTransform;
            return ScreenTools.WorldToCanvasPosition(Camera.main, worldPosition, rectTransform);
        }

        public bool IsPointOffScreen(Vector2 canvasPosition)
        {
            Vector2 screenSize = CanvasTransform.sizeDelta;

            if (canvasPosition.x < screenSize.x * -0.5f)
                return true;

            if (canvasPosition.x > screenSize.x * 0.5f)
                return true;

            if (canvasPosition.y > screenSize.y * 0.5f)
                return true;

            if (canvasPosition.y < screenSize.y * -0.5f)
                return true;

            return false;
        }
    }
}