using UnityEngine;

public class StunState : EnemyState
{
    private float _currentTime;
    private float stunTime;

    public StunState(float duration)
    {
        stunTime = duration;
    }

    public override void EnterState(Enemy enemy)
    {
        base.EnterState(enemy);
    }
    public override void UpdateState(Enemy enemy)
    {
        _currentTime += Time.deltaTime;
        if(_currentTime >= stunTime)
        {
            //Swap to other state check if onattak onchase on idle
            if(_enemy.movements.InRangeToChase())
            {
                _enemy.events.Following();
            }

            _currentTime = 0;
        }
    }

    public override void ExitState(Enemy enemy)
    {
        base.ExitState(enemy);
    }
}
