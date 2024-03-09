using UnityEngine;

public class DeadState : EnemyState
{
    public override void EnterState(Enemy enemy)
    {
        base.EnterState(enemy);
    }

    public override void UpdateState(Enemy enemy)
    {
        if(_enemy.onAir)
        {
            enemy.movements.ApplyCustomGravity(10f);
            if(enemy.movements.CheckGround(enemy))
            {
                enemy.onAir = false;
                enemy.movements.EnableAgent();
            }
        }

    }

    public override void ExitState(Enemy enemy)
    {
        base.ExitState(enemy);
    }
}
