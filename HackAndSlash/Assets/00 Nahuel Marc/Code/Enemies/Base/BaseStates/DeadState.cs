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
        _counter += Time.deltaTime;
        if (_counter >= 2.5f)
        {
            Destroy(enemy.gameObject);
        }
    }

    public override void ExitState(Enemy enemy)
    {
        _counter = 0;
        base.ExitState(enemy);
    }
}
