using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public class SlashesController : MonoBehaviour
{
    protected PlayerManager _player;
    public GameObject rightHand, leftHand;

    protected virtual void Start()
    {
        _player = GetComponent<PlayerManager>();
    } 

    protected virtual void InstantiateMiddleBody(GameObject instantiable)
    {
        GameObject go = Instantiate(instantiable, GetPosToInstantiate(1), transform.rotation);
        go.AddComponent<DealDamage>().damage = _player.stats.baseDamage;
    }

    protected virtual Vector3 GetPosToInstantiate(float height)
    {
        return new Vector3(transform.position.x, transform.position.y + height, transform.position.z);
    }

    // This function given a float number returns two separated numbers int part and decimal part
    // Example: Given 50.1 returns 50 and 1, given 37.5 returns 37 and 5
    protected virtual (int, int) GetNumber(float value)
    {
        int entero = Mathf.FloorToInt(value);
        float numDecimal = value - entero;
        if (numDecimal == 0)
        {
            return (entero, 0);
        }
        else
        {
            int parteDecimal = (int)(numDecimal * 10);
            return (entero, parteDecimal);
        }

    }
}
