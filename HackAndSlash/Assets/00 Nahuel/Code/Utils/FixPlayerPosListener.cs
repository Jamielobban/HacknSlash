using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixPlayerPosListener : MonoBehaviour
{
    Transform _target;
    void Start()
    {
        _target = GameManager.Instance.Player.transform;
    }

    void Update()
    {
        transform.position = _target.position;
    }
}
