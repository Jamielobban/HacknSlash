using UnityEngine;

public class AirState : EnemyState
{
    private bool isGrounded;
    private float _elapsedGravityDelay = 0f;

    public override void EnterState(Enemy enemy)
    {
        base.EnterState(enemy);
    }
    public override void UpdateState(Enemy enemy)
    {
        //Delay to active gravity
        _elapsedGravityDelay += Time.deltaTime;
        if(_elapsedGravityDelay >= enemy.stats.timeToActiveGravity.Value)
        {
            enemy.movements.EnableGravity();
            _elapsedGravityDelay = 0f;
        }


        //OnTouch ground swap to idle state
        isGrounded = Physics.Raycast(enemy.transform.position, Vector3.down, 0.1f);
        if(isGrounded)
        {
            enemy.movements.EnableAgent();
            enemy.SetState(new IdleState());
        }
    }

    public void ResetGravity(Enemy e)
    {
        e.movements.DisableGravity();
        _elapsedGravityDelay = 0f;
    }

    public override void ExitState(Enemy enemy)
    {
        base.ExitState(enemy);
        _elapsedGravityDelay = 0f;
        isGrounded = true;
    }
}
