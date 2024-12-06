using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Helper class for starting and stopping coroutines decoupled from Monobehavior. <br/>
/// Call start and stop methods using CoroutineManager.Instance.
/// </summary>
public class CoroutineManager : MonoBehaviour, ICoroutineManager
{
    public static ICoroutineManager Instance
    {
        get
        {
            if(_instance == null)
            {
                GameObject coroutineManagerObject = new GameObject("Coroutine Manager");
                DontDestroyOnLoad(coroutineManagerObject);
                _instance = coroutineManagerObject.AddComponent<CoroutineManager>();
            }

            return _instance;
        }
    }
    private static ICoroutineManager _instance = null;

    public new Coroutine StartCoroutine(IEnumerator coroutine)
    {
        return base.StartCoroutine(coroutine);
    }

    public new void StopCoroutine(Coroutine coroutine)
    {
        base.StopCoroutine(coroutine);
    }
}
