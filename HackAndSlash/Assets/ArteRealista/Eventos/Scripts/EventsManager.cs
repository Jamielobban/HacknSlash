using UnityEngine;
using System.Linq;

public class EventsManager : MonoBehaviour
{
    [SerializeField] GameObject[] eventsPrefabs;
    Transform[] eventsHolders;
    public int secondsToUnlockBase = 150;
    private void Awake()
    {        
        eventsHolders = GameObject.FindGameObjectsWithTag("Event").Select(go => go.transform).ToArray();

        int secondsIncrementToUnlock = secondsToUnlockBase;
        for (int i = 0; i < eventsHolders.Length; i++)
        {
            GameObject go = Instantiate(eventsPrefabs[Random.Range(0, eventsPrefabs.Count())], eventsHolders[i]);
            if(GameManager.Instance.state == Enums.GameState.Tutorial)
            {
                go.GetComponent<EventMap>().timeToActivate = 15;
            }
            else
            {
                go.GetComponent<EventMap>().timeToActivate = secondsIncrementToUnlock;
            }
            secondsIncrementToUnlock += secondsToUnlockBase;
        }
    }

}
