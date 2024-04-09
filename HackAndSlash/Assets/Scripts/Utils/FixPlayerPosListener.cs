using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixPlayerPosListener : MonoBehaviour
{
    Transform _target;
    void Start()
    {
        _target = GameManager.Instance.Player.transform;
        if(_target == null)
        {
            _target = FindObjectOfType<PlayerControl>().transform;
        }
    }

    void Update()
    {
        if(_target == null)
        {
            _target = FindObjectOfType<PlayerControl>().transform;
        }
        transform.position = _target.position;
    }
}
