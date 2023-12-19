using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemyHud hud { get; private set; }
    public EnemyMovement movements { get; private set; }

    private void Awake()
    {
        hud = GetComponent<EnemyHud>();
        movements = GetComponent<EnemyMovement>();
    }

    void Start()
    {

    }

    void Update()
    {
        
    }
}
