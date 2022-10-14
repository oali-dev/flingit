using System.Diagnostics;
using UnityEngine;

public static class DebugLogger
{
    [Conditional("UNITY_EDITOR")]
    public static void Log(object message)
    {
        UnityEngine.Debug.Log(message);
    }

    [Conditional("UNITY_EDITOR")]
    public static void Log(object message, Object context)
    {
        UnityEngine.Debug.Log(message, context);
    }
}
