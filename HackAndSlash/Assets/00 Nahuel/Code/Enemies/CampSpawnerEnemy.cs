using UnityEngine;

public class CampSpawnerEnemy : MonoBehaviour
{
    public CampManager _campParent;
    private Enemy _enemy;

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
    }

    private void Start()
    {
        _enemy.events.OnDie += () => { _campParent.RemoveEnemy(gameObject); };
    }

}
