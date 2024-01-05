using UnityEngine;

public class AirState : EnemyState
{
    private bool isGrounded;
    private float _elapsedGravityDelay = 0f;

    public override void EnterState(Enemy enemy)
    {
        base.EnterState(enemy);
        Debug.Log("Enter Air");
        enemy.events.OnHit += () => ResetGravity();
        enemy.movements.ThrowToAir();
    }
    public override void UpdateState(Enemy enemy)
    {
        //Delay to active gravity
        _elapsedGravityDelay += Time.deltaTime;
        if(_elapsedGravityDelay >= enemy.stats.timeToActiveGravity.Value)
        {
            enemy.movements.ApplyCustomGravity(2.0f);
        }
        //OnTouch ground swap to idle state
        isGrounded = Physics.Raycast(enemy.transform.position, Vector3.down, 0.04f);
        if(isGrounded)
        {
            enemy.movements.EnableAgent();
            enemy.events.Idle();
            ResetGravity();
        }
    }
    public void ResetGravity()
    {
        //e.movements.DisableGravity();
        _elapsedGravityDelay = 0f;
    }

    public override void ExitState(Enemy enemy)
    {
        isGrounded = true;
        base.ExitState(enemy);
    }
}
