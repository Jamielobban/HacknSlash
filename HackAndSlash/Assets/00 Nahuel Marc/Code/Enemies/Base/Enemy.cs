using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// -- Base Class for each enemy -- //
public class Enemy : MonoBehaviour
{
    public IState currentState;
    public EnemyController controller { get; private set; }

    public virtual void SetState(IState newState)
    {
        if(currentState == newState)
        {
            return;
        }
        currentState?.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }

    protected virtual void Awake()
    {
        controller = GetComponent<EnemyController>();
        SetState(new IdleState());
    }

    protected virtual void Start()
    {
        
    }
    protected virtual void FixedUpdate()
    {

    }
    protected virtual void Update()
    {
        currentState?.UpdateState(this);
    }
}
