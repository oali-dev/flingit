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
    [SerializeField]
    private GameObject _ballDestroyEffectPrefab;
    [SerializeField]
    private GameObject _fireworksEffectPrefab;
    [SerializeField]
    private GameObject _pauseMenu;
    [SerializeField]
    private ButtonController _pauseMenuMainMenuButtonController;
    [SerializeField]
    private ButtonController _pauseMenuRetryButtonController;
    [SerializeField]
    private ButtonController _hintButtonController;
    [SerializeField]
    private TextMeshProUGUI _hintButtonText;
    [SerializeField]
    private GameObject _hints;
    [SerializeField]
    private ButtonController _tutorialCloseButtonController;
    [SerializeField]
    private GameObject _congratulationsAnimationObject;

    public static bool IsQuitting = false;

    private InputManager _inputManager = null;
    private LevelInstance _levelInstance = null;
    private Coroutine GameWonSequenceCoroutine = null;

    private const float TimeBetweenEachFirework = 0.4f;

    private enum GameEndResult
    {
        WON,
        LOST
    }

    private void Awake()
    {
        _inputManager = new InputManager(_starController, _camera, _pauseMenu, _pauseMenuMainMenuButtonController, _pauseMenuRetryButtonController, _tutorialCloseButtonController);
        _levelInstance = new LevelInstance(_levelData);

        _starController.HookUpCollisionCallbacks(
            () => {
                bool isGameLost =_levelInstance.DecrementHitsAllowed();

                // Shake the camera whenever the ball collides with the wall
                // If the collision caused the level to fail then play an even stronger camera shake
                if(isGameLost)
                {
                    _cameraShakeScript.ShakeCamera(CameraShake.ShakeStrength.STRONG);
                    GameObject.Instantiate(_ballDestroyEffectPrefab, _starController.transform.position, Quaternion.identity);
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

        // Read from player prefs to see if we turn on hints and set up the button that turns hints on and off
        bool areHintsTurnedOn = (PlayerPrefs.GetInt("HintsOn", 0)) == 0 ? false : true;
        ToggleHints(areHintsTurnedOn);
        _hintButtonController._onButtonPress += () => {
            bool areHintsTurnedOn = (PlayerPrefs.GetInt("HintsOn", 0)) == 0 ? false : true;

            // If hints were on before pressing the button they are off after pressing it, and vice-versa
            ToggleHints(!areHintsTurnedOn);
            int hintsOnPlayerPrefs = areHintsTurnedOn ? 0 : 1;
            PlayerPrefs.SetInt("HintsOn", hintsOnPlayerPrefs);
        };
    }

    private void Update()
    {
        _inputManager.ProcessInput();
    }

    private void OnDestroy()
    {
        // When we quit the game unity tries to access CoroutineManager when it is already destroyed for some reason
        // Hence we check if the game is not quitting before running the original code here
        if(GameWonSequenceCoroutine != null && !IsQuitting)
        {
            CoroutineManager.Instance.StopCoroutine(GameWonSequenceCoroutine);
        }
    }

    private void OnApplicationQuit()
    {
        IsQuitting = true;
    }

    private void CheckIfGameHasEnded(bool isGameOver, GameEndResult gameEndResult)
    {
        if(isGameOver) {
            _starController.DestroyStar();

            if(gameEndResult == GameEndResult.WON)
            {
                PlayerPrefs.SetInt("HighestLevelUnlocked", _levelData.level + 1);
                if(_levelData.level == 20)
                {
                    GameWonSequenceCoroutine = CoroutineManager.Instance.StartCoroutine(PlayLastLevelGameWonSequence());
                }
                else
                {
                    GameWonSequenceCoroutine = CoroutineManager.Instance.StartCoroutine(PlayGameWonSequence());
                }
            }
            else
            {
                _retryButton.gameObject.SetActive(true);
            }
        }
    }

    private void ToggleHints(bool areHintsTurnedOn)
    {
        if(areHintsTurnedOn)
        {
            _hints.SetActive(true);
            _hintButtonText.SetText("HINTS: ON");
        }
        else
        {
            _hints.SetActive(false);
            _hintButtonText.SetText("HINTS: OFF");
        }
    }

    private IEnumerator PlayGameWonSequence()
    {
        // Yield just a little bit for tempo
        yield return new WaitForSeconds(TimeBetweenEachFirework);

        // Play Fireworks at 5 random spots on the screen
        for(int i = 0; i < 5; i++)
        {
            float x = Random.Range(0.2f, 0.8f);
            float y = Random.Range(0.2f, 0.8f);
            Vector3 worldPoint = _camera.ViewportToWorldPoint(new Vector3(x, y, _camera.nearClipPlane));
            GameObject.Instantiate(_fireworksEffectPrefab, worldPoint, Quaternion.identity);
            yield return new WaitForSeconds(TimeBetweenEachFirework);
        }

        _nextLevelButton.gameObject.SetActive(true);
    }

    private IEnumerator PlayLastLevelGameWonSequence()
    {
        // Yield just a little bit for tempo
        yield return new WaitForSeconds(TimeBetweenEachFirework);

        _congratulationsAnimationObject.SetActive(true);

        // Play Fireworks at 5 random spots on the screen
        for(int i = 0; i < 14; i++)
        {
            float x = Random.Range(0.2f, 0.8f);
            float y = Random.Range(0.2f, 0.8f);
            Vector3 worldPoint = _camera.ViewportToWorldPoint(new Vector3(x, y, _camera.nearClipPlane));
            GameObject.Instantiate(_fireworksEffectPrefab, worldPoint, Quaternion.identity);
            yield return new WaitForSeconds(TimeBetweenEachFirework);
        }

        _nextLevelButton.gameObject.SetActive(true);
    }
}
