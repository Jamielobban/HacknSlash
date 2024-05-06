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
    private bool eventStarted = false;

    protected override void Awake()
    {
        base.Awake();
        player = GameManager.Instance.Player.transform;
    }

    protected override void StartEvent()
    {
        base.StartEvent();
        eventStarted = true;
    }

    protected override void Update()
    {
        if(eventStarted)
        {
            _currentTime += Time.deltaTime;
            _currentTimeMeteor += Time.deltaTime;

            if (_currentTimeMeteor >= timeToSpawnMeteor)
            {
                GameObject go = Instantiate(meteorToSpawn, player.position, Quaternion.identity);
                go.transform.localRotation = Quaternion.Euler(-90, 0, 0);
                _currentTimeMeteor = 0;
            }

            if (_currentTime >= eventDuration)
            {
                CompleteEvent();
            }
        }
    }
}
