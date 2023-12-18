
using UnityEngine;

public class AirState : EnemyState
{
    private bool isGrounded;
    public override void EnterState(Enemy enemy)
    {
        base.EnterState(enemy);
    }
    public override void UpdateState(Enemy enemy)
    {
        //OnTouch ground swap to other state
        isGrounded = Physics.Raycast(enemy.transform.position, Vector3.down, 0.1f);
        if(isGrounded)
        {
            enemy.movements.EnableAgent();
            enemy.SetState(new IdleState());
        }
    }

    public override void ExitState(Enemy enemy)
    {
        base.ExitState(enemy);
    }
}
