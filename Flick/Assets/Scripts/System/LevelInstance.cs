using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps track of the state of a level.
/// </summary>
public class LevelInstance
{
    public int _numberOfHitsAllowed { get; private set; }
    public int _orbsRequiredToPassLevel { get; private set; }

    public LevelInstance(LevelData levelData)
    {
        _numberOfHitsAllowed = levelData.numberOfHitsAllowed;
        _orbsRequiredToPassLevel = levelData.orbsRequiredToPassLevel;
    }

    /// <summary>
    /// Decrements the number of allowed hits after a collision with an obstacle.
    /// </summary>
    /// <returns>True if we exceeded the number of hits allowed and the game is over.</returns>
    public bool DecrementHitsAllowed()
    {
        DebugLogger.Log("Hit Wall");
        _numberOfHitsAllowed -= 1;
        return (_numberOfHitsAllowed < 0);
    }

    /// <summary>
    /// Decrements the number of orbs required to pass the level after one is collected.
    /// </summary>
    /// <returns>True if we collected the required number of orbs and passed the level.</returns>
    public bool DecrementOrbsRequired()
    {
        DebugLogger.Log("Hit Point");
        _orbsRequiredToPassLevel--;
        return (_orbsRequiredToPassLevel == 0);
    }
}
