using UnityEngine;
using UnityEngine.UI;

public class LevelUnlocker : MonoBehaviour
{
    public Button[] levelSelectButtons;
    void Start()
    {
        int highestLevelUnlocked = PlayerPrefs.GetInt("HighestLevelUnlocked", 1);
        for(int i = highestLevelUnlocked; i < levelSelectButtons.Length; i++)
        {
            levelSelectButtons[i].interactable = false;
        }
    }
}
