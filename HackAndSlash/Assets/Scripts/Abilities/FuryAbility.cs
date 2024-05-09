using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuryAbility : MonoBehaviour
{
    public GameObject[] effect;
    PlayerControl player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.Instance.Player;   
    }

    private void OnEnable()
    {
        if(player == null) 
            player = GameManager.Instance.Player;

        effect[0].SetActive(true);
        effect[1].SetActive(true);

        player.attackBoost = 2;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
