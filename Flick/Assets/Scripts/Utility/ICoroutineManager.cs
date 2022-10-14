using System.Collections;
using UnityEngine;

/// <summary>
/// Interface for starting and stopping coroutines decoupled from Monobehavior.
/// </summary>
public interface ICoroutineManager
{
    public Coroutine StartCoroutine(IEnumerator coroutine);
    public void StopCoroutine(Coroutine coroutine);
}
