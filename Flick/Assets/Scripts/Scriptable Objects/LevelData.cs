using UnityEngine;

[CreateAssetMenu]
public class LevelData : ScriptableObject
{
    public int level;
    public int numberOfHitsAllowed;
    public int orbsRequiredToPassLevel;
    public int numberOfForceFieldsAllowed;
}