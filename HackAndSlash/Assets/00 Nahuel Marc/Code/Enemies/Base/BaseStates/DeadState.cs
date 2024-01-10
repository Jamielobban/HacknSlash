using UnityEngine;

public class DeadState : EnemyState
{
    private float _counter = 0f;
    public override void EnterState(Enemy enemy)
    {
        base.EnterState(enemy);
        Debug.Log("Enter Dead");
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
        _counter = 0;
        base.ExitState(enemy);
    }
}
