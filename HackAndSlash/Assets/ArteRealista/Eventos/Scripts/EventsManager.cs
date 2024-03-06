using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EventsManager : MonoBehaviour
{
    [SerializeField] GameObject[] eventsPrefabs;
    Transform[] eventsHolders;
    private void Awake()
    {
        eventsHolders = GameObject.FindGameObjectsWithTag("Event").Select(go => go.transform).ToArray();
        foreach (Transform t in eventsHolders) {
            Instantiate(eventsPrefabs[Random.Range(0, eventsPrefabs.Count())], t);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
