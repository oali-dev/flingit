using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public LevelData levelData;

    private InputManager _inputManager = null;
    private LevelInstance _levelInstance = null;

    private void Awake()
    {
        GameObject starGameObject = GameObject.Find("Star");
        StarController starController = starGameObject.GetComponent<StarController>();
        _inputManager = new InputManager(starController);
        _levelInstance = new LevelInstance(levelData);
    }

    private void Update()
    {
        _inputManager.ProcessInput();
    }
}
