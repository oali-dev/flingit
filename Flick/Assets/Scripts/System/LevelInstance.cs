using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps track of the state of a level.
/// </summary>
public class LevelInstance
{
    private int _numberOfHitsAllowed;
    private int _orbsRequiredToPassLevel;

    public LevelInstance(LevelData levelData)
    {
        _numberOfHitsAllowed = levelData.numberOfHitsAllowed;
        _orbsRequiredToPassLevel = levelData.orbsRequiredToPassLevel;
    }

    /// <summary>
    /// Decrements the number of allowed hits after a collision with an obstacle.
    /// </summary>
    /// <param name="amount">How much to decrement by</param>
    /// <returns>True if we exceeded the number of hits allowed and the game is over.</returns>
    public bool DecrementHitsAllowed(int amount = 1)
    {
        _numberOfHitsAllowed -= amount;
        return (_numberOfHitsAllowed < 0);
    }

    /// <summary>
    /// Decrements the number of orbs required to pass the level after one is collected.
    /// </summary>
    /// <returns>True if we collected the required number of orbs and passed the level.</returns>
    public bool DecrementOrbsRequired()
    {
        _orbsRequiredToPassLevel--;
        return (_orbsRequiredToPassLevel == 0);
    }
}
