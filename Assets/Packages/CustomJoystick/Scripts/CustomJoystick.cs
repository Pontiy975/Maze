using DG.Tweening;
using Maze.Game;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomJoystick : MonoBehaviour
{

    private enum TouchPointMode
    {
        Static,
        Dynamic
    }

    [SerializeField] private GameStateMachine gamestateMachine;
    [SerializeField] private Image backImage;
    [SerializeField] private Image middleImage;

    [SerializeField] private RectTransform middle;

    [SerializeField] private TouchPointMode touchPointModeMode = TouchPointMode.Static;
    //[SerializeField] private ScreenManager screenManager;

    private RectTransform _rect;

    private Vector3 _touchPosition;
    private bool _isTouched = false;

    private Vector3 _directionInPixels;
    private Vector3 _direction;

    private float _directionMagnitude;

    private RectTransform _canvasRect;

    void Start()
    {
        _rect = GetComponent<RectTransform>();
        _canvasRect = transform.parent.GetComponent<RectTransform>();
    }

    private void ShowJoy()
    {
        backImage.gameObject.SetActive(true);
        middleImage.gameObject.SetActive(true);

        Color white = new Color(1f, 1f, 1f, 0.3f);
        backImage.DOColor(white, 0.2f);
        middleImage.DOColor(white, 0.2f);
    }

    private void HideJoy()
    {
        backImage.DOColor(new Color(1f, 1f, 1f, 0f), 0.2f);
        middleImage.DOColor(new Color(1f, 1f, 1f, 0f), 0.2f);
    }
    
    private bool IsCanStartTouch()
    {
#if UNITY_EDITOR
        return Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject();
#else
        return Input.touchCount > 0 && !EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId);
#endif
    }
    
    private bool IsCanContinueTouch()
    {
#if UNITY_EDITOR
        return Input.GetMouseButton(0);
#else
        return Input.touchCount > 0;
#endif
    }

    void Update()
    {
        if (!gamestateMachine.CheckStates(GameState.InGame))
        {
            if (_isTouched)
                EndTouch();

            return;
        }

        if (!_canvasRect)
            return;

        if(!_isTouched && IsCanStartTouch())
            StartTouch();

        if (_isTouched && !IsCanContinueTouch())
        {
            EndTouch();
            return;
        }
        
        _directionInPixels = Input.mousePosition - _touchPosition;
        _direction = _directionInPixels * (1536f / Screen.width);

        _directionMagnitude = _direction.magnitude;

        if (_directionMagnitude > 180f)
        {
            if (touchPointModeMode == TouchPointMode.Dynamic && !Application.isEditor)
            {
                _touchPosition += _direction - _direction.normalized * 180f;

                RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRect, _touchPosition, null,
                    out Vector2 screenPos);
                _rect.anchoredPosition = screenPos;
            }

            _direction *= 180f / _directionMagnitude;
        }
        
        middle.anchoredPosition = _direction;
        
        _direction /= 180f;
        _directionMagnitude = _direction.magnitude;
    }

    private void StartTouch()
    {
        if(_isTouched)
            return;
        
        ShowJoy();

        _isTouched = true;
        _touchPosition = Input.mousePosition;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRect, _touchPosition, null, out Vector2 screenPos);
        _rect.anchoredPosition = screenPos;
    }

    private void EndTouch()
    {
        if(!_isTouched)
            return;
        
        HideJoy();
        _isTouched = false;
        _directionMagnitude = 0;
    }

    public bool HasInput => _isTouched && DirectionXZ != Vector3.zero;

    public bool IsTouched => _isTouched;

    public Vector3 DirectionXZ => new(_direction.x, 0f, _direction.y);
    public float DirectionMagnitude => _directionMagnitude;
}
