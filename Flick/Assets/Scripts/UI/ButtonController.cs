using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    private int _currentLevel = 1;

    public void SetCurrentLevel(int level)
    {
        _currentLevel = level;
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene("Level" + _currentLevel);
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene("Level" + (_currentLevel + 1));
    }
}
