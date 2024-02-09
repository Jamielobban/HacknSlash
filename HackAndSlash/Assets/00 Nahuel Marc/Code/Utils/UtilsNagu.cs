using System.Collections.Generic;
using UnityEngine;

public static class UtilsNagu 
{
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
