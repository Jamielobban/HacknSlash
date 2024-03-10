using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

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

    public static bool DoListsMatch<T>(List<T> list1, List<T> list2)
    {
        var areListsEqual = true;

        if (list1.Count != list2.Count)
            return false;

        //list1.Sort(); // Sort list one
        //list2.Sort(); // Sort list two

        for (var i = 0; i < list1.Count; i++)
        {
            if (!EqualityComparer<T>.Default.Equals(list2[i], list1[i]))
            {
                areListsEqual = false;
            }
        }

        return areListsEqual;
    }

    public static void RemoveAllNulls<T>(ref List<T> list) where T : class
    {
        list.RemoveAll(item => item == null);
    }

    public static void  RemoveAllInactive (ref List<GameObject> list)
    {
        List<GameObject> withoutInactive = new List<GameObject>();
        foreach (GameObject item in list)
        {
            if (item.activeSelf && item != null)
            {
                withoutInactive.Add(item);
            }
        }
        list = withoutInactive;
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
