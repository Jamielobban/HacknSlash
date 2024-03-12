using UnityEngine;
using System.Linq;
using TMPro;

public class EventsManager : MonoBehaviour
{
    [SerializeField] GameObject[] eventsPrefabs;
    Transform[] eventsHolders;
    public int secondsToUnlockBase = 150;

    int numEventsToComplete = 0;
    int currentCompletedEvents = 0;
    public TextMeshProUGUI eventsText;


    private void Awake()
    {        
        eventsHolders = GameObject.FindGameObjectsWithTag("Event").Select(go => go.transform).ToArray();

        int secondsIncrementToUnlock = secondsToUnlockBase;
        for (int i = 0; i < eventsHolders.Length; i++)
        {
            GameObject go = Instantiate(eventsPrefabs[Random.Range(0, eventsPrefabs.Count())], eventsHolders[i]);
            EventMap eventScrit = go.GetComponent<EventMap>();

            eventScrit.manager = this;

            if(GameManager.Instance.state == Enums.GameState.Tutorial)
            {
                eventScrit.timeToActivate = 15;
            }
            else
            {
                eventScrit.timeToActivate = secondsIncrementToUnlock;
            }
            secondsIncrementToUnlock += secondsToUnlockBase;
        }

        numEventsToComplete = eventsHolders.Length;
    }

    private void Update()
    {
        if(CheckAllEventsCompleted())
        {
            //Si todos eventos completos unlock final boss / win
        }
    }

    public void SetCurrentCompletedEvents()
    {
        currentCompletedEvents++;

        if(eventsText != null)
        {
            eventsText.text = "Events: " + currentCompletedEvents + " / " + numEventsToComplete;
        }
    }


    public bool CheckAllEventsCompleted() => currentCompletedEvents >= numEventsToComplete;

}
