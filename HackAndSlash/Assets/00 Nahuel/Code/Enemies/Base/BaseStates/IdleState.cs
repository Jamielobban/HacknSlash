using UnityEngine;
using UnityEngine.AI;

public class IdleState : EnemyState
{
    public override void EnterState(Enemy enemy)
    {
        base.EnterState(enemy);
    }

    public override void UpdateState(Enemy enemy)
    {
        NavMeshPath path = new NavMeshPath();
        bool isPathValid = _enemy.movements.Agent.CalculatePath(_enemy.movements.target.position, path);
        Debug.Log("Handle idle" + isPathValid);

        if (enemy.movements.InRangeToChase() && isPathValid)
        {
            enemy.events.Following();
        }
    }

    public override void ExitState(Enemy enemy)
    {
        base.ExitState(enemy);
    }
}
