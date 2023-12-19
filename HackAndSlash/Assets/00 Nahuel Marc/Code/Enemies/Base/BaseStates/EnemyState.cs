using UnityEngine;

// -- Base Class for enemy states. parent -> patroll, attack, etc. -- //

public abstract class EnemyState : MonoBehaviour, IState
{
    protected Enemy _enemy;
    public virtual void EnterState(Enemy enemy)
    {
        _enemy = enemy;
    }
    public abstract void UpdateState(Enemy enemy);
    public virtual void ExitState(Enemy enemy)
    {
        // Handle Exit State
    }

}
