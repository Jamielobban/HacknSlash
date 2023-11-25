using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartelChallenges : MonoBehaviour
{
    public List<Challenges1> Challenges1;
    public List<Challenges2> Challenges2;

    public List<Challenges> Challenges;

    ControllerManager controller;
    ChallengeManager manager;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < Challenges1.Count;i++)
        {
            Challenges.Add(Challenges1[i]);
        }
        for (int i = 0; i < Challenges2.Count; i++)
        {
            Challenges.Add(Challenges2[i]);
        }

        controller = GameObject.FindObjectOfType<ControllerManager>();
        manager = GameObject.FindObjectOfType<ChallengeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void GetRandomChallenge()
    {
        if (manager.currentChallenges.Count < 1 && Challenges.Count > 0)
        {
            int rand = Random.RandomRange(0, Challenges.Count - 1);

            switch (Challenges[rand].tipe)
            {
                case ChallengeTipe.CONSECUTIVE:

                    ((Challenges2)Challenges[rand]).currentCount = 0;
                    break;
                case ChallengeTipe.DEATHATTACK:
                    ((Challenges1)Challenges[rand]).currentCount = 0;


                    break;
            }

            manager.currentChallenges.Add(Challenges[rand]);
            manager.ShowNewChallenge(Challenges[rand]);
            Challenges.RemoveAt(rand);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(controller.GetInteractButton().action != null)
        {
            if (controller.GetInteractButton().action.WasPressedThisFrame())
            {
                GetRandomChallenge();
            }
        }
    }
}
