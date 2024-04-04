using UnityHFSM;

public class MeleeAttack : BaseEnemyAttack
{
    public void OnAttack(State<Enums.EnemyStates, Enums.StateEvent> state)
    {
        transform.LookAt(_player.transform.position);
        Use();
    }
    // public void OnAttack(State<Enums.EnemyStates, Enums.StateEvent> state)
    // {
    //     transform.LookAt(_player.transform.position);
    //     Use();
    // }
}
