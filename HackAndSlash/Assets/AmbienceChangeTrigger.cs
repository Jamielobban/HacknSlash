using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceChangeTrigger : MonoBehaviour
{
    public string parameterName;
    public float parameterValue;    
    public string parameterName2;
    public float parameterValue2;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Player"))
        {
            AudioManager.Instance.SetAmbienceParameter(parameterName, parameterValue);
            AudioManager.Instance.SetAmbienceParameter(parameterName2, parameterValue2);
        }
    }
}
