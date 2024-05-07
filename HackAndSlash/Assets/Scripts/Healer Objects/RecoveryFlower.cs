using UnityEngine;
using UnityEngine.Events;

public class RecoveryFlower : StructureEnemiesLife
{
    public GameObject gemsOfPower;
    public GameObject[] treeLeafs;
    public int reusedLifes = 3;
    public UnityEvent enabledTrigger;
    protected override void Awake()
    {
        gemsOfPower.SetActive(false);
    }
    protected override void Update()
    {
        if (CanActivate())
        {
            Activate();
        }
        else if ( reusedLifes <= 0 )
        {
            if (treeLeafs[0].activeSelf)
            {
                for (int i = 0; i < treeLeafs.Length; i++)
                {
                    treeLeafs[i].SetActive(false);
                }
            }
        }
    }

    protected override bool CanActivate() => base.CanActivate() && reusedLifes > 0;

    protected override void Activate()
    {
        base.Activate();
        if(!gemsOfPower.activeSelf)
        {
            gemsOfPower.SetActive(true);
            reusedLifes--;
            enabledTrigger.Invoke();
        }
    }

    public override void ResetStructure()
    {
        base.ResetStructure();
        gemsOfPower.SetActive(false);
    }
}
