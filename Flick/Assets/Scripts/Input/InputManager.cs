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

    private TouchProcessor _touchProcessor = null;
    private StarController _starController = null;
    private Camera _camera = null;
    private GameObject _pauseMenu = null;
    private bool _isGamePaused;
    private ButtonController _tutorialCloseButton = null;
    private bool _isTutorialShowing;

    private readonly int _tappableLayerMask = LayerMask.GetMask("Tappable");

    public InputManager(StarController starController, Camera camera, GameObject pauseMenu, ButtonController pauseMenuMainMenuButtonController, ButtonController pauseMenuRetryButtonController, ButtonController tutorialCloseButton)
    {
        _touchProcessor = new IdleTouchProcessor(starController);
        _starController = starController;
        _camera = camera;
        _pauseMenu = pauseMenu;
        _isGamePaused = false;
        pauseMenuMainMenuButtonController._onButtonPress += () => { Time.timeScale = 1.0f; };
        pauseMenuRetryButtonController._onButtonPress += () => { Time.timeScale = 1.0f; };
        if(tutorialCloseButton != null)
        {
            _isTutorialShowing = true;
            tutorialCloseButton._onButtonPress += () => { _isTutorialShowing = false; };
        }
        else
        {
            _isTutorialShowing = false;
        }
    }

    public void ProcessInput()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(_isGamePaused)
            {
                UnpauseGame();
            }
            else
            {
                PauseGame();
            }
        }

        if(_isGamePaused)
        {
            return;
        }

        if(_isTutorialShowing)
        {
            return;
        }

        TouchInfo touchInfo = GetTouchInfo();
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

    private TouchInfo GetTouchInfo()
    {
        Vector2 worldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);

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

    private Collider2D DetectCollider(Vector2 worldPosition)
    {
        return Physics2D.OverlapPoint(worldPosition, _tappableLayerMask);
    }

    private void PauseGame()
    {
        _isGamePaused = true;
        Time.timeScale = 0f;
        _pauseMenu.SetActive(true);
    }

    private void UnpauseGame()
    {
        _isGamePaused = false;
        Time.timeScale = 1.0f;
        _pauseMenu.SetActive(false);
    }
}
