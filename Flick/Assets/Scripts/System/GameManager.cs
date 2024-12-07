using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private LevelData _levelData;
    [SerializeField]
    private StarController _starController;
    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private TextMeshProUGUI _hitsRemainingText;
    [SerializeField]
    private ButtonController _retryButton;
    [SerializeField]
    private ButtonController _nextLevelButton;
    [SerializeField]
    private CameraShake _cameraShakeScript;

    private InputManager _inputManager = null;
    private LevelInstance _levelInstance = null;

    private enum GameEndResult
    {
        WON,
        LOST
    }

    private void Awake()
    {
        _inputManager = new InputManager(_starController, _camera);
        _levelInstance = new LevelInstance(_levelData);

        _starController.HookUpCollisionCallbacks(
            () => {
                bool isGameLost =_levelInstance.DecrementHitsAllowed();

                // Shake the camera whenever the ball collides with the wall
                // If the collision caused the level to fail then play an even stronger camera shake
                if(isGameLost)
                {
                    _cameraShakeScript.ShakeCamera(CameraShake.ShakeStrength.STRONG);
                }
                else
                {
                    _cameraShakeScript.ShakeCamera(CameraShake.ShakeStrength.WEAK);
                }

                _hitsRemainingText.SetText(_levelInstance._numberOfHitsAllowed.ToString());
                CheckIfGameHasEnded(isGameLost, GameEndResult.LOST);
            }, 
            () => {
                bool isGameWon =_levelInstance.DecrementOrbsRequired();
                CheckIfGameHasEnded(isGameWon, GameEndResult.WON);
            }
        );
        _starController._numberOfForceFieldsAllowed = _levelData.numberOfForceFieldsAllowed;

        _hitsRemainingText.SetText(_levelData.numberOfHitsAllowed.ToString());
        _retryButton.SetCurrentLevel(_levelData.level);
        _nextLevelButton.SetCurrentLevel(_levelData.level);
    }

    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();

        // Create collidable borders along each edge of the camera using an edge collider
        // Need to call after WaitForEndOfFrame because Pixel Perfect Camera changes the viewport in LateUpdate
        CreateCameraBorderColliders();
    }

    private void Update()
    {
        _inputManager.ProcessInput();
    }

    private void CreateCameraBorderColliders()
    {
        Vector2 bottomLeft = _camera.ViewportToWorldPoint(new Vector3(0, 0, _camera.nearClipPlane));
        Vector2 topLeft = _camera.ViewportToWorldPoint(new Vector3(0, 1, _camera.nearClipPlane));
        Vector2 topRight = _camera.ViewportToWorldPoint(new Vector3(1, 1, _camera.nearClipPlane));
        Vector2 bottomRight = _camera.ViewportToWorldPoint(new Vector3(1, 0, _camera.nearClipPlane));

        Vector2[] edgeColliderPoints = new[] { bottomLeft, topLeft, topRight, bottomRight, bottomLeft };
        List<Vector2> pointList = new List<Vector2>(edgeColliderPoints);
        EdgeCollider2D edgeCollider = _camera.gameObject.GetComponent<EdgeCollider2D>();
        edgeCollider.SetPoints(pointList);
        edgeCollider.enabled = true;
    }

    private void CheckIfGameHasEnded(bool isGameOver, GameEndResult gameEndResult)
    {
        if(isGameOver) {
            _starController.DestroyStar();

            if(gameEndResult == GameEndResult.WON)
            {
                _nextLevelButton.gameObject.SetActive(true);
            }
            else
            {
                _retryButton.gameObject.SetActive(true);
            }
        }
    }
}
