using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public Button startButton;
    public Button settingsButton;

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        startButton = root.Q<Button>("start-button");
        settingsButton = root.Q<Button>("settings-button");

        startButton.clicked += () => SceneManager.LoadScene("TestScene2");
    }
}
