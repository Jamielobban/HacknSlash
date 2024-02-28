using UnityEngine;

public class PoolableObject : MonoBehaviour
{
    public ObjectPool parent;
    public bool isPooleable = true;
    public virtual void OnDisable()
    {
        if(isPooleable)
        {
            parent.ReturnObjectToPool(this);
        }
    }
}
