using UnityEngine;

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

    public void SpawnSlash(int isRightHand)
    {

        GameObject go = Instantiate(slashNormalFX, transform.position, Quaternion.LookRotation(transform.forward));

        go.transform.rotation = isRightHand == 1 ? rightHand.transform.rotation : leftHand.transform.rotation;

        go.AddComponent<DealDamage>().damage = _player.stats.baseDamage;
    }

    public void Spawn360Slash()
    {
        GameObject go = Instantiate(slash360FX, transform.position, Quaternion.identity);
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
    }
}
