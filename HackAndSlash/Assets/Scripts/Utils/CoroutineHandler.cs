using System;
using UnityEngine;
using System.Collections;

public class CoroutineHandler : MonoBehaviour
{
    private static CoroutineHandler _instance;
    public static CoroutineHandler Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("CoroutineHandler").AddComponent<CoroutineHandler>();
            }
            return _instance;
        }
    }
    public Coroutine StartCoroutineMine(IEnumerator coroutine)
    {
        return base.StartCoroutine(coroutine);
    }
    public void StopCoroutineMine(Coroutine coroutine)
    {
        base.StopCoroutine(coroutine);
    }

    private void OnDestroy()
    {
        if(this == _instance)
        {
            _instance = null;
        }
    }
}
