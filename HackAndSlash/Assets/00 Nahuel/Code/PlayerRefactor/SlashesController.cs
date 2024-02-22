using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public class SlashesController : MonoBehaviour
{
    protected PlayerManager _player;
    public GameObject rightHand, leftHand;



    public GameObject slashNormalToRight, slashNormalToLeft;
    public GameObject slash360FX;
    public GameObject yingYangFX;
    public GameObject thrustFX, thrustFxBig;

    protected virtual void Start()
    {
        _player = GetComponent<PlayerManager>();
    }

    public void SpawnSlash(float value)
    {
        (int anguloX, int isRightHand) = GetNumber(value);
        GameObject go = Instantiate(slashNormalToRight, isRightHand == 1 ? rightHand.transform.position : leftHand.transform.position, Quaternion.LookRotation(transform.forward));
        go.transform.rotation = Quaternion.Euler(new Vector3(anguloX, -180, transform.rotation.z));
        go.AddComponent<DealDamage>().damage = _player.stats.baseDamage;
    }

    public void SpawnToLeftSlash(float value)
    {
        (int anguloX, int isRightHand) = GetNumber(value);
        GameObject go = Instantiate(slashNormalToRight, isRightHand == 1 ? rightHand.transform.position : leftHand.transform.position, Quaternion.LookRotation(transform.forward));
        go.transform.rotation = Quaternion.Euler(new Vector3(anguloX, -180, 180));
        go.AddComponent<DealDamage>().damage = _player.stats.baseDamage;
    }

 



    public void Spawn360Slash()
    {
        InstantiateMiddleBody(slash360FX);
    }

    public void SpawnSmallThrustSlash(int isRightHand)
    {
        GameObject go = Instantiate(thrustFX, isRightHand == 1 ? rightHand.transform.position : leftHand.transform.position, Quaternion.LookRotation(transform.forward));
        go.GetComponent<DealDamage>().damage = _player.stats.baseDamage;
    }

    public void SpawnThrustSlash(int isRightHand)
    {
        GameObject go = Instantiate(thrustFxBig, isRightHand == 1 ? rightHand.transform.position : leftHand.transform.position, Quaternion.LookRotation(transform.forward));
        go.GetComponent<DealDamage>().damage = _player.stats.baseDamage;
    }

    public void SpawnYingYang()
    {
        GameObject go = Instantiate(yingYangFX, transform.position, Quaternion.identity);
        go.transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, -90, transform.rotation.z));
    }

    protected virtual void InstantiateMiddleBody(GameObject instantiable)
    {
        GameObject go = Instantiate(slash360FX, GetPosToInstantiate(1), Quaternion.identity);
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
