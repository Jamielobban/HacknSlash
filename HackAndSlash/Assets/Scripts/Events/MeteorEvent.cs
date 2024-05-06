using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorEvent : EventBase
{
    public float eventDuration;
    public float timeToSpawnMeteor;
    public GameObject meteorToSpawn;

    private float _currentTimeMeteor;
    private float _currentTime;

    private Transform player;

    protected override void Awake()
    {
        base.Awake();
        player = GameManager.Instance.Player.transform;
    }

    protected override void Update()
    {
        _currentTime += Time.deltaTime;
        _currentTimeMeteor  += Time.deltaTime;

        if(_currentTimeMeteor >= timeToSpawnMeteor)
        {
            _currentTimeMeteor = 0;
        }

        if(_currentTime >= eventDuration)
        {
            CompleteEvent();
        }
    }
}
