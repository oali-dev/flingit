using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class ButtonController : MonoBehaviour
{
    public delegate void OnButtonPressCallback();
    public OnButtonPressCallback _onButtonPress;
    private int _currentLevel = 1;

    // Parse the number at the end of the scene name to get the current level
    private void Start()
    {
        string currentLevelString = SceneManager.GetActiveScene().name;
        int i = currentLevelString.Length - 1;
        while(char.IsDigit(currentLevelString[i]))
        {
            i--;
            if(i < 0)
            {
                DebugLogger.Log("Invalid scene named used for level");
                break;
            }
        }

        // Return the index to when the char at i was a valid digit
        i++;

        // If there were no numbers at the end of the scene name then we check if the scene is MainMenu
        if(i == (currentLevelString.Length))
        {
            if(currentLevelString == "MainMenu")
            {
                _currentLevel = 0;
            }
            else
            {
                DebugLogger.Log("Invalid scene named used for level");
                return;
            }
        }
        else
        {
            _currentLevel = Int32.Parse(currentLevelString.Substring(i));
        }
    }

    public void LoadMainMenu()
    {
        if(_onButtonPress != null)
        {
            _onButtonPress();
        }
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadLevel()
    {
        if(_onButtonPress != null)
        {
            _onButtonPress();
        }
        SceneManager.LoadScene("Level" + _currentLevel);
    }

    public void LoadNextLevel()
    {
        if(_onButtonPress != null)
        {
            _onButtonPress();
        }
        SceneManager.LoadScene("Level" + (_currentLevel + 1));
    }
}
