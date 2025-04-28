using UnityEngine;
using Zenject;
using System;

public class MobileInputService : IInitializable, ITickable
{
    public event Action OnSwipeUp;
    public event Action<Vector2> OnDrag;
    public bool IsTouching => _isTouching;

    private Vector2 _startTouchPos;
    private bool _isTouching;
    private float _swipeThreshold = 50f;

    public void Initialize() {}

    public void Tick()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _startTouchPos = touch.position;
                    _isTouching = true;
                    break;

                case TouchPhase.Moved:
                    Vector2 delta = touch.position - _startTouchPos;
                    OnDrag?.Invoke(delta);
                    break;

                case TouchPhase.Ended:
                    _isTouching = false;
                    if ((touch.position - _startTouchPos).y > _swipeThreshold)
                        OnSwipeUp?.Invoke();
                    break;
            }
        }
        else
        {
            _isTouching = false;
        }
    }
}
