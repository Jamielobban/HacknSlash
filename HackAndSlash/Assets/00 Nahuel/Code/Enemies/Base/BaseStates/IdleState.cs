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
        if (Time.frameCount % 20 == 0 && _enemy._player.states != PlayerControl.States.JUMP)
        {
            _enemy.movements._path = new NavMeshPath();
            _enemy.movements._isPathValid = _enemy.movements.Agent.CalculatePath(_enemy.movements.target.position, _enemy.movements._path);
        }

        if (enemy.movements.InRangeToChase() && _enemy.movements._isPathValid)
        {
            enemy.events.Following();
        }
    }

    public override void ExitState(Enemy enemy)
    {
        base.ExitState(enemy);
    }
}
