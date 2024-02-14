using System;
using System.Collections.Generic;
using UnityEngine;

public static class UtilsNagu 
{
    //Value for space out small dists
    public const float Epsilon = 0.001f;
    public const float PI_MEDIO = (float)Math.PI / 2;
    public static Vector3 GetCameraForward(Camera cam)
    {
        Vector3 forward = cam.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }
    public static Vector3 GetCameraRight(Camera cam)
    {
        Vector3 right = cam.transform.right;
        right.y = 0;
        return right.normalized;
    }

    public static bool ListContainsAnotherListFromStart<T>(List<T> listaPrincipal, List<T> listaSecundaria)
    {
        if (listaSecundaria.Count > listaPrincipal.Count)
        {
            return false; 
        }

        for (int i = 0; i < listaSecundaria.Count; i++)
        {
            if (!EqualityComparer<T>.Default.Equals(listaPrincipal[i], listaSecundaria[i]))
            {
                return false; 
            }
        }

        return true; 
    }

    public static void RemoveAllNulls<T>(ref List<T> list)
    {
        List<T> withoutNulls = new List<T>();
        foreach (T item in list)
        {
            if(item != null)
            {
                withoutNulls.Add(item);
            }
        }
        list = withoutNulls;
    }

    public static T RandomWeightedElement<T>(this IEnumerable<T> collection) where T : EnemyProbability
    {
        var random = UnityEngine.Random.value * 100;

        var current = 0f;
        foreach(var element in collection)
        {
            if(current <= random && random < current + element.probability)
            {
                return element;
            }
            current += element.probability;
        }
        return default(T);
    }
}
