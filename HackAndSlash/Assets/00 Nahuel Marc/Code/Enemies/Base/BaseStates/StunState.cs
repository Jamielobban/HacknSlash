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
        Debug.Log("Enter Stun");

        _currentTime = 0f;
    }
    public override void UpdateState(Enemy enemy)
    {
        _currentTime += Time.deltaTime;
        if(_currentTime >= stunTime)
        {
            _enemy.events.Idle();
            _currentTime = 0f;
        }
    }

    public override void ExitState(Enemy enemy)
    {
        _currentTime = 0f;
        base.ExitState(enemy);
    }
}
