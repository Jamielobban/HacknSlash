using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB_SlashesController : SlashesController
{
    public GameObject slashNormalToRight;

    public GameObject slash360FX, slashL2Square360FX;
    public GameObject yingYangFX;
    public GameObject thrustFX, thrustFxBig;

    public GameObject SlashSquare;

    public List<GameObject> slashesSquare = new List<GameObject>();
    public List<GameObject> slashesL2Square = new List<GameObject>();
    public List<GameObject> slashesAnother = new List<GameObject>();

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

    public void SpawnAnotherSlash(int index)
    {

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
        go.transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, -90, transform.rotation.z));
    }

    public void Spawn360Slash()
    {
        InstantiateMiddleBody(slash360FX);
    }

}
