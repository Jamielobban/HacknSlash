using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class NaveMov : MonoBehaviour
{
    bool move = false;
    [SerializeField] float velocity;
    public void StartMove() => move = true;   
    void Update()
    {
        if(move)
            this.transform.position = this.transform.position + new Vector3(velocity * Time.deltaTime, 0,0);
    }
}
