using UnityEngine;

public class AirState : EnemyState
{
    private float _elapsedGravityDelay = 0f;

    public override void EnterState(Enemy enemy)
    {
        base.EnterState(enemy);
        Debug.Log("Enter Air");
        enemy.events.OnHit += ResetGravity;
        enemy.events.OnDie += () => _elapsedGravityDelay = 50f;
        _elapsedGravityDelay = 0f;
        enemy.onAir = true;
        enemy.movements.DisableMovement();
        enemy.movements.DisableAgent();
        enemy.movements.Throw();
    }
    public override void UpdateState(Enemy enemy)
    {
        _elapsedGravityDelay += Time.deltaTime;

        if(_elapsedGravityDelay >= enemy.stats.timeToActiveGravity.Value)
        {
            enemy.movements.ApplyCustomGravity(10f);

            //OnTouch ground swap to idle state
            if(enemy.movements.CheckGround(enemy)) 
            {
                enemy.onAir = false;
                ResetGravity();
                enemy.movements.EnableAgent();
                enemy.events.Idle();
            }
        }
    }
    public void ResetGravity()
    {
        _enemy.movements.GetRigidBody().velocity = Vector3.zero;
        _elapsedGravityDelay = 0f;
    } 
    public override void ExitState(Enemy enemy)
    {
        enemy.animations.Animator.SetTrigger("endAir");
        enemy.events.OnHit -= ResetGravity;
        base.ExitState(enemy);
    }
}
