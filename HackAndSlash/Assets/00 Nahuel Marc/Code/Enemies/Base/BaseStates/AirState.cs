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
        _elapsedGravityDelay = 0f;

        enemy.movements.DisableMovement();
        enemy.movements.DisableAgent();
        enemy.movements.ThrowToAir();
    }
    public override void UpdateState(Enemy enemy)
    {
        _elapsedGravityDelay += Time.deltaTime;

        if(_elapsedGravityDelay >= enemy.stats.timeToActiveGravity.Value)
        {
            enemy.movements.ApplyCustomGravity(1.5f);

            //OnTouch ground swap to idle state
            RaycastHit hit;
            if (Physics.Raycast(enemy.transform.position, Vector3.down, out hit, 0.1f, LayerMask.GetMask("Suelo")))
            {
                if (hit.collider.GetComponent<TerrainCollider>() != null)
                {
                    // Restablecer la gravedad y activar el agente
                    ResetGravity();
                    enemy.movements.EnableAgent();
                    enemy.events.Idle();
                }
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
        Debug.Log("Exit Air State");
        enemy.animations.Animator.SetTrigger("endAir");
        enemy.events.OnHit -= ResetGravity;
        base.ExitState(enemy);
    }
}
