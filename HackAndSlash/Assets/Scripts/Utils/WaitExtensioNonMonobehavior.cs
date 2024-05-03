using System;
using System.Collections;
using UnityEngine;

public static class WaitExtensioNonMonobehavior
{
    public static void Wait(float delay, Action action)
    {
        CoroutineHandler.Instance.StartCoroutineMine(ExecuteAction(delay, action));
    }

    private static IEnumerator ExecuteAction(float delay, Action action)
    {
        yield return new WaitForSecondsRealtime(delay);
        action.Invoke();
    }
}
