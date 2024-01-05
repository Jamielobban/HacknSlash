using UnityEngine;

public class PatrollState : EnemyState
{
    public override void EnterState(Enemy enemy)
    {
        base.EnterState(enemy);
        Debug.Log("Enter Patroll");

    }
    public override void UpdateState(Enemy enemy)
    {
        enemy.movements.HandlePatrollInArea();
        if(enemy.movements.InRangeToChase())
        {
            enemy.events.Following();
        }
    }

    public override void ExitState(Enemy enemy)
    {
        Debug.Log("Exit State Patroll");
        base.ExitState(enemy);
    }
}
