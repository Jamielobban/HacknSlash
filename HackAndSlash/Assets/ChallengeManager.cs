using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChallengeManager : MonoBehaviour
{
    //public List<Challenges> currentChallenges;
    ControllerManager controller;

    public GameObject menus;

    public TextMeshProUGUI description;

    public TextMeshProUGUI current;
    public TextMeshProUGUI target;

    DeathEnemies enemiesDeath;

    public GameObject menusShow;
    public TextMeshProUGUI descriptionShow;

    public GameObject menusComplete;

    bool active;
    // Start is called before the first frame update
    void Start()
    {
        active = false;
        controller = GameObject.FindAnyObjectByType<ControllerManager>();
        enemiesDeath = GameObject.FindAnyObjectByType<DeathEnemies>();
    }

    public void ChallengeComplete(int challenge)
    {
        menus.SetActive(false);

        // currentChallenges.RemoveAt(challenge);
        menusComplete.SetActive(true);
        Invoke("DesaparecerChallengeComplete", 2);

    }
    public void DesaparecerChallengeComplete()
    {
        menusComplete.SetActive(false);

    }
    /*
    public void ShowNewChallenge(Challenges challenge)
    {
        if(active)
            menus.SetActive(true);

        menusShow.SetActive(true);
        descriptionShow.text = challenge.description.ToString();

        Invoke("DesaparecerNewChallenge",2);
    }*/
    public void DesaparecerNewChallenge()
    {
        menusShow.SetActive(false);

    }
    /*
    void UpdateMenus()
    {
        if (currentChallenges.Count <= 0)
            return;
        switch (currentChallenges[0].tipe)
        {
            case ChallengeTipe.CONSECUTIVE:
                description.text = (currentChallenges[0]).description.ToString();

                Challenges2 challenge1 = (Challenges2)currentChallenges[0];
                current.text = enemiesDeath.consecutiveDeaths.ToString();

                target.text = challenge1.count.ToString();

                break;
            case ChallengeTipe.DEATHATTACK:
                description.text = (currentChallenges[0]).description.ToString();
                
                Challenges1 challenge2 = (Challenges1)currentChallenges[0];
                current.text = challenge2.currentCount.ToString();

                target.text = challenge2.count.ToString();

                break;
        }
    }*/
    // Update is called once per frame
    void Update()
    {
        //UpdateMenus();

        //    if (controller.GetTabButton().action != null)
        //    {
        //        if (controller.GetTabButton().action.WasPressedThisFrame() && currentChallenges.Count > 0)
        //        {
        //            if (!active)
        //            {
        //                active = true;

        //                menus.SetActive(true);

        //            }
        //            else
        //            {
        //                active = false;
        //                menus.SetActive(false);


        //            }
        //        }
        //    }
        //}
    }
}
