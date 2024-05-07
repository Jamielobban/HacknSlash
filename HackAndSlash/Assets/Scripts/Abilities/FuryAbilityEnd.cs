using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuryAbilityEnd : MonoBehaviour
{
    PlayerControl player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.Instance.Player;   
    }

    private void OnEnable()
    {
        Invoke("End", 10);
    }

    void End()
    {
        player.attackBoost = 1;
        this.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
