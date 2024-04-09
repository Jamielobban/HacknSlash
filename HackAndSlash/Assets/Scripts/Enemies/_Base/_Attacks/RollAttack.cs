using UnityEngine;
using UnityHFSM;

public class RollAttack : BaseEnemyAttack
{
    [SerializeField] private GameObject _sensor;
    public void OnRoll(State<Enums.EnemyStates, Enums.StateEvent> state)
    {
        _sensor.gameObject.SetActive(true);
        transform.LookAt(_player.transform.position);
        Use();
    }
}
