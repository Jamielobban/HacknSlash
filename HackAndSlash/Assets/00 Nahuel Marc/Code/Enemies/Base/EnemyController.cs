using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemyHealthSystem healthSystem { get; private set; }
    public EnemyHud hud { get; private set; }

    public EnemyMovement movements { get; private set; }
    public Enemy enemy { get; private set; }

    private void Awake()
    {
        hud = GetComponent<EnemyHud>();
        enemy = GetComponent<Enemy>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
