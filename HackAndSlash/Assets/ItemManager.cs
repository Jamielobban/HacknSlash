using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;

    //public List<GameObject> itemPool = new List<GameObject>();
    public int test;
    public List<GameObject> itemPool = new List<GameObject>();
    private void Awake()
    {
        // Singleton pattern to ensure only one instance exists
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(ItemTooltip);
        }
        else
        {
            Destroy(this);
        }

        // Initialize other manager-specific setup here
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Call this method when you want to spawn a random item
    public void SpawnRandomItem(Vector3 initialPosition)
    {
        //bool spawnOnce = false;
        if (itemPool.Count == 0)
        {
            Debug.LogWarning("Item pool is empty. Add items to the pool.");
            return;
        }
        //if (!spawnOnce)
        //{

            // Instantiate the item at the initial position
            GameObject randomItem = Instantiate(itemPool[Random.Range(0, itemPool.Count)], initialPosition, Quaternion.identity);
            Vector3 playerForward = GameObject.FindObjectOfType<PlayerControl>().transform.GetChild(0).transform.forward;
            Vector3 randomDirection = Quaternion.Euler(0, Random.Range(-45, 45), 0) * playerForward;
            Vector3 targetPosition = initialPosition + randomDirection * Random.Range(5f, 10f);
            RaycastHit hit;

            if (Physics.Raycast(targetPosition, new Vector3(0,-1,0), out hit, 200, 1 << 7))
            {
                targetPosition = new Vector3(targetPosition.x, hit.point.y + 2, targetPosition.z);
            }
            //spawnOnce = true;
                // Use DoTween to animate the item from the initial to the target position
                randomItem.transform.DOJump(targetPosition, 2.0f, 1, 2.0f)
                 .SetEase(Ease.OutQuart)
                 .OnComplete(() =>
                 {
                     // Animation complete, item can be picked up or further processed
                     randomItem.GetComponent<SphereCollider>().enabled = true;
                     randomItem.GetComponent<BounceEffect>().enabled = true;
                 });
        //}
    }
}

