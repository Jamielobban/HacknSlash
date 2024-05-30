
using UnityEngine;

public class BombOnJump : Item
{
    public GameObject prefabToInstantiate;
    public float timeToInvoke = 0;
    public override void OnItemPickup(PlayerControl player)
    {
        ItemManager manager = FindObjectOfType<ItemManager>();

        if (manager.actionItems.Contains(this))
        {
            manager.actionItems.Remove(this);
        }
        player.OnJump += ActionToDo;
    }
    private void ActionToDo()
    {
        //Instancias bomba
        Invoke(nameof(InvokeBomb), timeToInvoke);

    }
    private void InvokeBomb()
    {
        GameObject bomb = Instantiate(prefabToInstantiate, GameManager.Instance.Player.transform.position, Quaternion.identity);
        bomb.GetComponent<ProjectileMover>().ShootToEnemy(Vector3.down);
    }

    private void OnDestroy()
    {
        GameManager.Instance.Player.OnJump -= ActionToDo;
    }
}
