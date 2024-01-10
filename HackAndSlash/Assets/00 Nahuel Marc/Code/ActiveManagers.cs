using UnityEngine;

public class ActiveManagers : MonoBehaviour
{
    private void Awake()
    {
        RoomManager.Instance.Active();
    }
}
