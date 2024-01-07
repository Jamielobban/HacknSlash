using UnityEngine;

public class AirState : EnemyState
{
    private bool isGrounded;
    private float _elapsedGravityDelay = 0f;

    public override void EnterState(Enemy enemy)
    {
        base.EnterState(enemy);
        Debug.Log("Enter Air");
        enemy.events.OnHit += ResetGravity;
        enemy.movements.DisableAgent();
        enemy.movements.ThrowToAir();
    }
    public override void UpdateState(Enemy enemy)
    {
        //Delay to active gravity
        _elapsedGravityDelay += Time.deltaTime;

        if(_elapsedGravityDelay >= enemy.stats.timeToActiveGravity.Value)
        {
            enemy.movements.ApplyCustomGravity(2.0f);

            //OnTouch ground swap to idle state
            isGrounded = Physics.Raycast(enemy.transform.position, Vector3.down, 0.04f, LayerMask.NameToLayer("Suelo"));

            if (isGrounded)
            {
                ResetGravity();
                enemy.movements.EnableAgent();
                enemy.events.Idle();
            }
        }

    }
    public void ResetGravity() => _elapsedGravityDelay = 0f;

    public override void ExitState(Enemy enemy)
    {
        Debug.Log("Exit Air State");
        enemy.animations.Animator.StopPlayback();
        enemy.events.OnHit -= ResetGravity;
        base.ExitState(enemy);
    }
}
