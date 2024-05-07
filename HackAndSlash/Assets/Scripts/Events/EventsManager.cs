using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public struct EventInfo
{
    public GameObject eventPrefab;
    public float timeToSpawn;
}

public class EventsManager : MonoBehaviour
{
    [SerializeField] private List<EventInfo> eventsInfos = new List<EventInfo>();
    [SerializeField] private List<GameObject> eventsPrefabs = new List<GameObject>();
    private int _indexEvent = 0;

    private Transform player;

    private void Awake()
    {
        player = GameManager.Instance.Player.transform;
        SetEvents();
    }

    private void Update()
    {
        if(_indexEvent <= eventsInfos.Count - 1)
        {
            if (LevelManager.Instance.CurrentGlobalTime >= eventsInfos[_indexEvent].timeToSpawn)
            {
                GameObject eventGo = Instantiate(eventsInfos[_indexEvent].eventPrefab, player.position, Quaternion.identity);

                _indexEvent++;
            }
        }
    }

    private void SetEvents()
    {
        for (int i = 0; i < eventsInfos.Count; i++)
        {
            if(eventsInfos[i].eventPrefab == null)
            {
                EventInfo info = eventsInfos[i];
                info.eventPrefab = eventsPrefabs[UnityEngine.Random.Range(0, eventsPrefabs.Count)];
                eventsInfos[i] = info;
            }
        }
    }
}
