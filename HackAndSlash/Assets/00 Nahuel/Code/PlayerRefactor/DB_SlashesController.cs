using System.Collections.Generic;
using UnityEngine;

public class DB_SlashesController : SlashesController
{
    public GameObject slashNormalToRight;
    public GameObject slashL2Square360FX;
    public GameObject yingYangFX;
    public GameObject thrustFX, thrustFxBig;

    public List<GameObject> slashesSquare = new List<GameObject>();
    public List<GameObject> slashesL2Square = new List<GameObject>();
    public List<GameObject> slashesL2Triangle = new List<GameObject>();
    public List<GameObject> slashesAnother = new List<GameObject>();

    public GameObject dragon;
    protected override void Start()
    {
        base.Start();
        InitializeDamages();
    }

    public void SpawnSquareSlash(int index)
    {
        if (slashesSquare[index] != null)
        {
            slashesSquare[index].SetActive(true);
        }
    }

    public void SpawnL2SquareSlash(int index)
    {
        if (slashesL2Square[index] != null)
        {
            slashesL2Square[index].SetActive(true);
        }
    }
    public void SpawnL2TriangleSlash(int index)
    {
        if (slashesL2Triangle[index] != null)
        {
            slashesL2Triangle[index].SetActive(true);
        }
    }

    public void SpawnAnotherSlash(int index)
    {

    }

    public void SpawnDragon()
    {
        GameObject go = Instantiate(dragon, new Vector3(transform.position.x + 0.5f, transform.position.y + 1, transform.position.z), transform.rotation);
        go.GetComponent<DealDamage>().damage = _player.stats.baseDamage;
    }

    //public void SpawnSlash(float value)
    //{
    //    (int anguloZ, int isRightHand) = GetNumber(value);
    //    GameObject go = Instantiate(slashNormalToRight, isRightHand == 1 ? rightHand.transform.position : leftHand.transform.position, transform.rotation);

    //    go.AddComponent<DealDamage>().damage = _player.stats.baseDamage;
    //}
    //public void SpawnToLeftSlash(float value)
    //{
    //    (int anguloZ, int isRightHand) = GetNumber(value);
    //    GameObject go = Instantiate(slashNormalToRight, isRightHand == 1 ? rightHand.transform.position : leftHand.transform.position, transform.rotation);
    //    go.AddComponent<DealDamage>().damage = _player.stats.baseDamage;
    //}

    public void SpawnSlashMiddle()
    {
        InstantiateMiddleBody(slashNormalToRight);
    }   
    public void SlashL2SquareMiddle()
    {
        InstantiateMiddleBody(slashL2Square360FX);
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

    private void InitializeDamages()
    {
        foreach (GameObject slash in slashesSquare)
        {
            slash.GetComponent<DealDamage>().damage = _player.stats.baseDamage;
        }

        foreach (GameObject slash in slashesL2Square)
        {
            slash.GetComponent<DealDamage>().damage = _player.stats.baseDamage;
        }

        foreach (GameObject slash in slashesL2Triangle)
        {
            slash.GetComponent<DealDamage>().damage = _player.stats.baseDamage;
        }
    }

}
