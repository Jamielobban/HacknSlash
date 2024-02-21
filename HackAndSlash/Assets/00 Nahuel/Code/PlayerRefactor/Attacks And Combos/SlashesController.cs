using UnityEngine;
using UnityEngine.UIElements;

public class SlashesController : MonoBehaviour
{
    public GameObject rightHand, leftHand;

    public GameObject slashNormalFX;
    public GameObject slash360FX;
    public GameObject yingYangFX;
    public GameObject thrustFX, thrustFxBig;
    private PlayerManager _player;

    private void Start()
    {
        _player = GetComponent<PlayerManager>();
    }

    public void SpawnSlash(float value)
    {
        (int angulo, int isRightHand) = GetNumber(value);
        GameObject go = Instantiate(slashNormalFX, isRightHand == 1 ? rightHand.transform.position : leftHand.transform.position, Quaternion.identity);

        //go.transform.rotation = isRightHand == 1 ? rightHand.transform.rotation : leftHand.transform.rotation;
        go.transform.rotation = Quaternion.Euler(new Vector3(angulo, -180, transform.rotation.z));
        go.AddComponent<DealDamage>().damage = _player.stats.baseDamage;
    }
    private Vector3 GetPosToInstantiate(float height)
    {
        return new Vector3(transform.position.x, transform.position.y + height, transform.position.z);  
    }

    private (int, int) GetNumber(float value)
    {
        int entero = Mathf.FloorToInt(value);
        float numDecimal = value - entero;
        if(numDecimal == 0)
        {
            return (entero, 0);
        }
        else
        {
            int parteDecimal = (int)(numDecimal * 10);
            return (entero, parteDecimal);
        }

    }
    public void Spawn360Slash()
    {
        GameObject go = Instantiate(slash360FX, GetPosToInstantiate(1), Quaternion.identity);
        go.AddComponent<DealDamage>().damage = _player.stats.baseDamage;
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
}
