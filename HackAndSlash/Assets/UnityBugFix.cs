using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityBugFix : MonoBehaviour
{
    [System.Serializable]
    public class Positions
    {
        public GameObject gameObject;
        public Vector3 position; // Change variable name to avoid confusion
    }

    public Positions[] positionsList;

    void Start()
    {
        foreach (Positions position in positionsList)
        {
            RectTransform rectTransform = position.gameObject.GetComponent<RectTransform>();

            if (rectTransform != null)
            {
                // Set the anchored position of the RectTransform
                rectTransform.anchoredPosition = position.position;
            }
            else
            {
                Debug.LogError("RectTransform not found on GameObject: " + position.gameObject.name);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // You can add any additional logic or updates here if needed
    }
}
