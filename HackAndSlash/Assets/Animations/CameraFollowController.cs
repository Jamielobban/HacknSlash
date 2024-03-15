using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowController : MonoBehaviour
{
    //public GetEnemies getEnemies;
    //private Vector3 startPosition = new Vector3 (1.92f, 1.46f, 0);
    //private PlayerControl playerControl;
    //void Start()
    //{
    //    getEnemies = FindObjectOfType<GetEnemies>();
    //    playerControl = FindObjectOfType<PlayerControl>();
    //    //this.transform.position = startPosition;
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    Vector3 averagePosition = CalculateAveragePosition(getEnemies.enemies,playerControl);
    //    if(getEnemies.enemies.Count > 0)
    //    {

    //        this.transform.position = averagePosition;
    //    }
    //    else
    //    {
    //        //this.transform.localPosition = startPosition;
    //    }
    //}

    //private Vector3 CalculateAveragePosition(List<GameObject> objects, PlayerControl player)
    //{
    //    Vector3 sum = Vector3.zero;

    //    // Iterate through each GameObject in the list
    //    foreach (GameObject obj in objects)
    //    {
    //        // Check if the GameObject has a Transform component
    //        if (obj != null && obj.TryGetComponent<Transform>(out Transform objTransform))
    //        {
    //            // Add the position to the sum
    //            sum += objTransform.position;
    //        }
    //        else
    //        {
    //            Debug.LogWarning("GameObject in the list does not have a Transform component.");
    //        }
    //    }

    //    // Add the player's position to the sum if the player script and player GameObject are valid
    //    if (player != null && player.gameObject != null)
    //    {
    //        sum += player.gameObject.transform.position;
    //    }

    //    // Calculate the average position
    //    Vector3 average = sum / (objects.Count + 1); // +1 to account for the player

    //    return average;
    //}
}
