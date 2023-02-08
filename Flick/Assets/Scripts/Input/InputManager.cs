using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    public enum TouchState
    {
        IDLE,
        START,
        DRAG,
        RELEASE
    }

    public struct TouchInfo
    {
        public Vector2 worldPosition;
        public TouchState touchState;
        public Collider2D collider;

        public TouchInfo(Vector2 worldPosition, TouchState touchState, Collider2D collider = null)
        {
            this.worldPosition = worldPosition;
            this.touchState = touchState;
            this.collider = collider;
        }
    }

    /// <summary>
    /// The currently active touch processor.
    /// </summary>
    private TouchProcessor _touchProcessor = null;

    private StarController _starController = null;

    /// <summary>
    /// Layer mask of all physics objects that are tappable, which currently is only the star.
    /// </summary>
    private readonly int _tappableLayerMask = LayerMask.GetMask("Tappable");

    public InputManager(StarController starController)
    {
        _touchProcessor = new IdleTouchProcessor(starController);
        _starController = starController;
    }

    public void ProcessInput()
    {
        TouchInfo touchInfo = GetTouch();
        if(_touchProcessor.HasEnded(touchInfo))
        {
            _touchProcessor = _touchProcessor.End(touchInfo);
            _touchProcessor.Start(touchInfo);
        }
        else
        {
            _touchProcessor.Update(touchInfo);
        }
    }

    private TouchInfo GetTouch()
    {
#if UNITY_MOBILE
        return GetMobileTouchInfo();
#elif UNITY_WEBGL || UNITY_EDITOR
        return GetDesktopTouchInfo();
#endif
    }

#if UNITY_MOBILE
    private TouchInfo GetMobileTouchInfo()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            TouchPhase phase = touch.phase;
            // TODO: Why is this not using mouse position from touch? Don't think this code works also touch position never declared anywhere
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if(phase == TouchPhase.Began)
            {
                return new TouchInfo(touchPosition, TouchState.START, DetectCollider(mousePosition));
            }   
            else if(phase == TouchPhase.Moved || phase == TouchPhase.Stationary)
            {
                return new TouchInfo(touchPosition, TouchState.DRAG);
            }
            else if(phase == TouchPhase.Ended || phase == TouchPhase.Canceled)
            {
                return new TouchInfo(touchPosition, TouchState.RELEASE, DetectCollider(mousePosition));
            }
        }

        return new TouchInfo(Vector2.zero, TouchState.IDLE);
    }
#endif

#if UNITY_WEBGL || UNITY_EDITOR
    private TouchInfo GetDesktopTouchInfo()
    {
        // TODO: Pass the main camera to InputManager and save it as a member variable
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if(Input.GetMouseButtonDown(0))
        {
            return new TouchInfo(worldPosition, TouchState.START, DetectCollider(worldPosition));
        }
        else if(Input.GetMouseButton(0))
        {
            return new TouchInfo(worldPosition, TouchState.DRAG);
        }
        else if(Input.GetMouseButtonUp(0))
        {
            return new TouchInfo(worldPosition, TouchState.RELEASE, DetectCollider(worldPosition));
        }

        return new TouchInfo(worldPosition, TouchState.IDLE);
    }
#endif

    private Collider2D DetectCollider(Vector2 worldPosition)
    {
        return Physics2D.OverlapPoint(worldPosition, _tappableLayerMask);
    }
}
